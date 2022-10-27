using System.Collections.Generic;
using UnityEngine;
using Board.Model;
using System;
using Random = UnityEngine.Random;
using System.Linq;

namespace Board.Controller
{
    public class BoardController : IDisposable
    {
        public BoardModel Model;
        private SkillController _skillController;
        private List<BoardInput> _inputs = new();
        private Skill _skillSelected;
        public BoardInput InputSelected;
        private GameProgressionService _gameProgression;
        private GameConfigService _gameConfigService;
        private MatchReport _matchReport;

        public event Action<Vector2Int, Vector2Int> OnEmblemMoved = delegate (Vector2Int origin, Vector2Int destination) { };
        public event Action<Vector2Int> OnEmblemDestroyed = delegate (Vector2Int emblemDestroyed) { };
        public event Action<Vector2Int, EmblemItem> OnEmblemCreated = delegate (Vector2Int emblemPosition, EmblemItem item) { };
        public event Action<Vector2Int, EmblemItem> OnColorChanged = delegate (Vector2Int emblemPosition, EmblemItem item) { };

        private TripleIntArgument_Event _onPlayerAttacks;
        public IntArgument_Event OnAvailableMovesChanged;
        private StringArgument_Event _onInputDisabled;

        public BoardController(int width, int height, SkillController skillController,
            List<BoardInput> inputList, GameProgressionService gameProgression, TripleIntArgument_Event OnPlayerAttacks,
            IntArgument_Event OnAvailableMovesChanged, GameConfigService gameConfigService, MatchReport matchReport,
            StringArgument_Event OnInputDisabled)
        {

            _onInputDisabled = OnInputDisabled;
            _onInputDisabled.AddListener(ChangeInput);
            _matchReport = matchReport;
            _gameConfigService = gameConfigService;
            this.OnAvailableMovesChanged = OnAvailableMovesChanged;
            _onPlayerAttacks = OnPlayerAttacks;
            _inputs = inputList;
            _gameProgression = gameProgression;
            _skillController = skillController;

            Model = new BoardModel(width, height);
        }

        public void Initialize()
        {
            ChangeInput("NormalInput");
            _skillSelected = _skillController.SkillSelected;
            _skillController.OnSkillActivated += ChangeInput;
            LoadHero();

            OnAvailableMovesChanged.TriggerEvents(CalculateEmblemsWithMatch().Count);
        }

        private void LoadHero()
        {
            List<HeroItemModel> allHeroesModel = _gameConfigService.HeroModel;
            Model.hero = GetHeroData(allHeroesModel);
        }

        private HeroItemModel GetHeroData(List<HeroItemModel> allHeroesModel)
        {
            foreach (HeroItemModel hero in allHeroesModel)
            {
                if (hero.Id == _gameProgression.GetSelectedHero()) return hero;
            }
            return null;
        }

        public void Dispose()
        {
            _skillController.OnSkillActivated -= ChangeInput;
            _onInputDisabled.RemoveListener(ChangeInput);
        }

        #region SKILLS

        public void TryUseSkill(Vector2Int touchPosition)
        {
            if (TouchIsWithinLimits(touchPosition))
            {
                _skillSelected.PerformSkill(this, touchPosition, Model.hero.Color);
            }
            ChangeInput("NormalInput");
        }

        public void ChangeInput(string inputId)
        {
            if (InputSelected.Id == "NoInput") return;
            InputSelected = _inputs.Find(input => input.Id == inputId);
        }

        #endregion

        public int GetEmblemColor(int x, int y)
        {
            return (int)Model.GetEmblem(x, y).Item.EmblemColor;
        }

        public void TryProcessMatch(Vector2Int touchPosition)
        {
            if (TouchIsWithinLimits(touchPosition)) ProcessMatch(touchPosition);
        }

        private void ProcessMatch(Vector2Int touchPosition)
        {
            EmblemModel touchedEmblem = Model.GetEmblem(touchPosition);

            if (touchedEmblem.IsEmpty()) return;

            List<EmblemModel> swapMatches = RecursiveSearch(touchedEmblem);

            if (swapMatches.Count > 1) DestroyAndCollapse(swapMatches);
        }

        public void DestroyAndCollapse(List<EmblemModel> comboMatches)
        {
            int colorAttack = GetEmblemColor(comboMatches[0].Position.x, comboMatches[0].Position.y);
            
            foreach (EmblemModel emblem in comboMatches)
            {
                Model.GetEmblem(emblem.Position).Item = null;

                OnEmblemDestroyed(emblem.Position);
            }
            VerticalCollapse();
            _onPlayerAttacks.TriggerEvents(comboMatches.Count, colorAttack, _matchReport.GetDestroyedColumns(this));
            _matchReport.AddMatchedEmblems(comboMatches.Count);
            _matchReport.AddMaxCombo(comboMatches.Count);
            HorizontalCollapse();
        }

        public void UpdateMoves()
        {
            int moves = CalculateEmblemsWithMatch().Count;
            OnAvailableMovesChanged.TriggerEvents(moves);
            if (moves == 0) RefillBoard();
        }

        public List<EmblemModel> CalculateEmblemsWithMatch()
        {
            List<EmblemModel> moves = new();

            for (int x = 0; x < Model.Width; x++)
            {
                for (int y = 0; y < Model.Height; y++)
                {
                    if (Model.GetEmblem(x, y).IsEmpty()) continue;
                    if (x < Model.Width - 1 && HasSameColor(Model.GetEmblem(x, y), Model.GetEmblem(x + 1, y)))
                    {
                        moves.Add(Model.GetEmblem(x, y));
                        moves.Add(Model.GetEmblem(x + 1, y));
                    }

                    if (y < Model.Height - 1 && HasSameColor(Model.GetEmblem(x, y), Model.GetEmblem(x, y + 1)))
                    {
                        moves.Add(Model.GetEmblem(x, y));
                        moves.Add(Model.GetEmblem(x, y + 1));
                    }
                }
            }
            moves.Distinct().ToList();
            return moves;
        }

        private void VerticalCollapse()
        {
            int nullCounter = 0;

            for (int x = 0; x < Model.Width; x++)
            {
                for (int y = 0; y < Model.Height; y++)
                {
                    if (Model.GetEmblem(x, y).IsEmpty())
                    {
                        nullCounter++;
                    }
                    else if (nullCounter > 0)
                    {
                        //Switch emblems in model 
                        EmblemItem aux = Model.GetEmblem(x, y - nullCounter).Item;
                        Model.GetEmblem(x, y - nullCounter).Item = Model.GetEmblem(x, y).Item;
                        Model.GetEmblem(x, y).Item = aux;

                        //Send view to new position
                        OnEmblemMoved(Model.GetEmblem(x, y).Position, Model.GetEmblem(x, y - nullCounter).Position);
                    }
                }
                nullCounter = 0;
            }
        }

        private void HorizontalCollapse()
        {
            if (!BoardIsSeparated()) return;

            int nullCounter = 0;

            for (int x = 0; x < Model.Width; x++)
            {
                if (Model.GetEmblem(x, 0).IsEmpty())
                {
                    nullCounter++;
                    continue;
                }
                else if (nullCounter > 0)
                {
                    for (int y = 0; y < Model.Height; y++)
                    {
                        EmblemItem aux = Model.GetEmblem(x - nullCounter, y).Item;
                        Model.GetEmblem(x - nullCounter, y).Item = Model.GetEmblem(x, y).Item;
                        Model.GetEmblem(x, y).Item = aux;

                        OnEmblemMoved(Model.GetEmblem(x, y).Position, Model.GetEmblem(x - nullCounter, y).Position);
                    }
                }
            }
        }

        public void RefillBoard()
        {
            for (int x = 0; x < Model.Width; x++)
            {
                for (int y = 0; y < Model.Height; y++)
                {
                    if (!Model.GetEmblem(x, y).IsEmpty()) continue;

                    //Create model emblem
                    Model.GetEmblem(x, y).Item = new EmblemItem()
                    {
                        EmblemColor = (EmblemColor)Random.Range(0, 5)
                    };

                    //Createm view emblem
                    OnEmblemCreated(Model.GetEmblem(x, y).Position, Model.GetEmblem(x, y).Item);

                }
            }
        }

        public void CreateEmblemAtPosition(Vector2Int position, int color)
        {
            for (int y = 0; y < Model.Height; y++)
            {
                if (!Model.GetEmblem(position.x, y).IsEmpty()) continue;

                Model.GetEmblem(position.x, y).Item = new EmblemItem()
                {
                    EmblemColor = (EmblemColor)color
                };

                OnEmblemCreated(Model.GetEmblem(position.x, y).Position, Model.GetEmblem(position.x, y).Item);
                break;
            }
        }

        public void ChangeEmblemColorAtPosition(Vector2Int position, int heroColor)
        {
            int emblemColor = (int)Model.GetEmblem(position.x, position.y).Item.EmblemColor;
            if (Model.GetEmblem(position.x, position.y).IsEmpty() || emblemColor == heroColor) return;

            Model.GetEmblem(position.x, position.y).Item.EmblemColor = (EmblemColor)heroColor;
            OnColorChanged(position, new EmblemItem((EmblemColor)heroColor));
        }

        private bool BoardIsSeparated()
        {
            for (int x = 0; x < Model.Width - 1; x++)
            {
                if (Model.GetEmblem(x, 0).IsEmpty())
                {
                    return true;
                }
            }
            return false;
        }

        private List<EmblemModel> RecursiveSearch(EmblemModel currentEmblem, List<EmblemModel> exclude = null)
        {
            List<EmblemModel> result = new List<EmblemModel> { currentEmblem };

            if (exclude == null)
            {
                exclude = new List<EmblemModel> { currentEmblem };
            }
            else
            {
                if (!exclude.Contains(currentEmblem)) exclude.Add(currentEmblem);
            }

            foreach (EmblemModel neighbour in GetNeighbours(currentEmblem))
            {
                if (neighbour.Item == null || exclude.Contains(neighbour) || neighbour.Item.EmblemColor != currentEmblem.Item.EmblemColor) continue;
                result.AddRange(RecursiveSearch(neighbour, exclude));
            }
            return result;
        }

        private List<EmblemModel> GetNeighbours(EmblemModel emblem)
        {
            List<EmblemModel> neighbourList = new();

            //Up
            if (emblem.Position.y < Model.Height - 1) neighbourList.Add(Model.GetEmblem(emblem.Position.x, emblem.Position.y + 1));

            //Down
            if (emblem.Position.y > 0) neighbourList.Add(Model.GetEmblem(emblem.Position.x, emblem.Position.y - 1));

            //Left
            if (emblem.Position.x > 0) neighbourList.Add(Model.GetEmblem(emblem.Position.x - 1, emblem.Position.y));

            //Right
            if (emblem.Position.x < Model.Width - 1) neighbourList.Add(Model.GetEmblem(emblem.Position.x + 1, emblem.Position.y));

            return neighbourList;
        }

        #region UTILITY METHODS

        private bool HasSameColor(EmblemModel emblem1, EmblemModel emblem2)
        {
            if (emblem1.IsEmpty() || emblem2.IsEmpty()) return false;
            else return emblem1.Item.EmblemColor == emblem2.Item.EmblemColor;
        }

        private bool TouchIsWithinLimits(Vector2Int touchPosition)
        {
            return (touchPosition.x >= 0 &&
                touchPosition.y >= 0 &&
                touchPosition.y < Model.Height &&
                touchPosition.x < Model.Width);
        }

        public EmblemModel GetEmblem(Vector2Int position)
        {
            return Model.GetEmblem(position);
        }
        #endregion
    }
}

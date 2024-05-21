using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using System.Linq;

public class CanvasHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TeamsHandler teamsHandler;
    [SerializeField] private ScoreHandler scoreHandler;

    [Header("Canvas Game Object")]
    [SerializeField] private GameObject _scoreTableMenu;
    [SerializeField] private GameObject _configurationMenu;
    [SerializeField] private GameObject _warningDeleteDataMenu; 
    [SerializeField] private GameObject _configurationButton;
    [SerializeField] private GameObject _backButton;
    [SerializeField] private GameObject _verticalLayoutToTeamsSlider;

    [Header("Canvas Classes")]
    [SerializeField] private TMP_InputField _scoreInput;
    [SerializeField] private TMP_InputField newTeamName;
    [SerializeField] private TMP_Dropdown teamsScoreDropDown;
    [SerializeField] private TMP_Dropdown teamsConfigurationDropDown;
    [SerializeField] private Image colorPickerColor;

    [Header("Prefabs")]
    [SerializeField] private GameObject _teamSliderPrefab;
    
    private Stack<ICommand> commandStack = new Stack<ICommand>();
    private List<GameObject> _teamSliders = new List<GameObject>();


    private void Awake()
    {
        _scoreTableMenu.SetActive(true);
        _configurationButton.SetActive(true);
        _backButton.SetActive(false);
        _configurationMenu.SetActive(false);
        _warningDeleteDataMenu.SetActive(false);
    }

    private void Start()
    {
        UpdateCanvas();
    }

    private void UpdateCanvas()
    {
        var data = DataPersistance.Instance.LoadJson();
        SetTeamsName(data.TeamsInfo);
        HandlerSliders(data.TeamsInfo, data.TotalScore);
    }

    public void AddScore()
    {
        var score = GetInputScore();
        var team = GetScoreSelectedTeam(teamsScoreDropDown, teamsScoreDropDown.value);

        scoreHandler.AddScore(team, score);
        
        _scoreInput.text = string.Empty;
        teamsScoreDropDown.value = 0;
        var data = DataPersistance.Instance.LoadJson();
        HandlerSliders(data.TeamsInfo, data.TotalScore);
    }

    public void SubtractScore()
    {
        var score = GetInputScore();
        var team = GetScoreSelectedTeam(teamsScoreDropDown, teamsScoreDropDown.value);

        scoreHandler.AddScore(team, -score);

        _scoreInput.text = string.Empty;
        teamsScoreDropDown.value = 0;
        var data = DataPersistance.Instance.LoadJson();
        HandlerSliders(data.TeamsInfo, data.TotalScore);
    }

    public void SetTeamsName(List<TeamsInfo> teamsData)
    {
        List<TMP_Dropdown.OptionData> optionConfigurationTeamDataList = new List<TMP_Dropdown.OptionData>();
        List<TMP_Dropdown.OptionData> optionScoreTeamDataList = new List<TMP_Dropdown.OptionData>();

        if (teamsConfigurationDropDown.options.Count > 0)
        {
            optionConfigurationTeamDataList.Add(teamsConfigurationDropDown.options[0]);
        }

        if (teamsScoreDropDown.options.Count > 0)
        {
            optionScoreTeamDataList.Add(teamsScoreDropDown.options[0]);
        }

        optionConfigurationTeamDataList.AddRange(teamsData.Select(team => new TMP_Dropdown.OptionData(team.TeamName, team.teamSprite)));
        optionScoreTeamDataList.AddRange(teamsData.Select(team => new TMP_Dropdown.OptionData(team.TeamName, team.teamSprite)));

        teamsConfigurationDropDown.options = optionConfigurationTeamDataList;
        teamsScoreDropDown.options = optionScoreTeamDataList;
        if (teamsConfigurationDropDown.options.Count > 0)
        {
            teamsConfigurationDropDown.options = optionConfigurationTeamDataList.Take(1).ToList();  // Keep the first item
            teamsConfigurationDropDown.options.AddRange(optionConfigurationTeamDataList.Skip(1));   // Add the rest of new items
        }
        else
        {
            teamsConfigurationDropDown.options = optionConfigurationTeamDataList;
        }

        if (teamsScoreDropDown.options.Count > 0)
        {
            teamsScoreDropDown.options = optionScoreTeamDataList.Take(1).ToList();  // Keep the first item
            teamsScoreDropDown.options.AddRange(optionScoreTeamDataList.Skip(1));   // Add the rest of new items
        }
        else
        {
            teamsScoreDropDown.options = optionScoreTeamDataList;
        }

        teamsScoreDropDown.RefreshShownValue();
        teamsConfigurationDropDown.RefreshShownValue();
    }

    public void HandlerSliders(List<TeamsInfo> teams, int totalScore)
    {
        while (_teamSliders.Count < teams.Count)
        {
            var sliderObj = Instantiate(_teamSliderPrefab, _verticalLayoutToTeamsSlider.transform);
            _teamSliders.Add(sliderObj);
        }

        for (int i = 0; i < teams.Count; i++)
        {
            var objSlider = _teamSliders[i].GetComponent<SliderHandler>();
            objSlider.SetName(teams[i].TeamName, teams[i].TeamColor, teams[i].teamSprite);
            objSlider.SetSliderValue(teams[i].TeamScore, totalScore);
            _teamSliders[i].SetActive(true);
        }

        for (int i = teams.Count; i < _teamSliders.Count; i++)
        {
            _teamSliders[i].SetActive(false);
        }
    }

    public void ModifyOrCreateTeam()
    {
        var teamColor = colorPickerColor.color;

        if (GetScoreSelectedTeam(teamsConfigurationDropDown, teamsConfigurationDropDown.value) == GetScoreSelectedTeam(teamsConfigurationDropDown, 0))
        {
            teamsHandler.AddTeam(newTeamName.text, teamColor, null);
        }
        else
        {
            var oldName = GetScoreSelectedTeam(teamsConfigurationDropDown, teamsConfigurationDropDown.value);
            var newName = newTeamName.text;

            if (newName == string.Empty && teamColor == Color.white)
                teamsHandler.DeleteTeam(oldName);
            else if(newName == string.Empty && teamColor != Color.white)
                teamsHandler.ModifyTeam(oldName, oldName, teamColor);
            else
                teamsHandler.ModifyTeam(oldName, newName, teamColor);
        }

        newTeamName.text = string.Empty;
        teamsConfigurationDropDown.value = 0;
        colorPickerColor.color = Color.white;
        UpdateCanvas();
    }

    private int GetInputScore()
    {
        if (int.TryParse(_scoreInput.text, out int result))
        {
            return result;
        }
        return 0;
    }

    private string GetScoreSelectedTeam(TMP_Dropdown dropdown, int index)
    {
        return dropdown.options[index].text;
    }

    [ContextMenu("GoToConfigurationNameMenu")]
    public void GoToConfigurationMenu()
    {
        ExecuteCommand(new ChangeMenuCommand(new[] { _configurationMenu, _backButton }, new[] { _scoreTableMenu, _configurationButton }));
    }

    [ContextMenu("GoToWarningMenu")]
    public void GoToWarningDeleteData()
    {
        ExecuteCommand(new ChangeMenuCommand(new[] { _warningDeleteDataMenu }, new[] { _configurationMenu, _backButton }));
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    [ContextMenu("Go Back")]
    public void GoBack()
    {
        UndoLastCommand();
    }

    public void AcceptDeleteData()
    {
        DataPersistance.Instance.DeleteData();
        UpdateCanvas();
    }

    private void ExecuteCommand(ICommand command)
    {
        command.Execute();
        commandStack.Push(command);
    }

    private void UndoLastCommand()
    {
        if (commandStack.Count > 0)
        {
            ICommand lastCommand = commandStack.Pop();
            lastCommand.Undo();
        }
    }

}

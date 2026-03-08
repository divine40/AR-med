using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class QuizManager : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text categoryText;
    public TMP_Text questionText;
    public TMP_Text feedbackText;
    public TMP_Text scoreText;

    public Button answerButton1;
    public Button answerButton2;
    public Button answerButton3;

    public TMP_Text answerText1;
    public TMP_Text answerText2;
    public TMP_Text answerText3;

    public Button nextButton;
    public Button restartButton;
    public Button menuButton;

    [System.Serializable]
    public class QuestionData
    {
        public string category;
        public string question;
        public string[] answers = new string[3];
        public int correctAnswerIndex;
    }

    [Header("Questions")]
    public QuestionData[] questions;

    private int currentQuestionIndex = 0;
    private int score = 0;
    private bool answered = false;

    void Start()
    {
        nextButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        menuButton.gameObject.SetActive(false);

        feedbackText.text = "";
        ShowQuestion();
        UpdateScoreUI();
    }

    void ShowQuestion()
    {
        if (currentQuestionIndex >= questions.Length)
        {
            ShowFinalResult();
            return;
        }

        answered = false;
        feedbackText.text = "";
        nextButton.gameObject.SetActive(false);

        QuestionData q = questions[currentQuestionIndex];

        categoryText.text = q.category;
        questionText.text = q.question;
        answerText1.text = q.answers[0];
        answerText2.text = q.answers[1];
        answerText3.text = q.answers[2];

        answerButton1.gameObject.SetActive(true);
        answerButton2.gameObject.SetActive(true);
        answerButton3.gameObject.SetActive(true);

        answerButton1.interactable = true;
        answerButton2.interactable = true;
        answerButton3.interactable = true;
    }

    public void SelectAnswer(int answerIndex)
    {
        if (answered) return;

        answered = true;

        QuestionData q = questions[currentQuestionIndex];

        if (answerIndex == q.correctAnswerIndex)
        {
            feedbackText.text = "Correct!";
            feedbackText.color = Color.green;
            score++;
            UpdateScoreUI();
        }
        else
        {
            string correctAnswer = q.answers[q.correctAnswerIndex];
            feedbackText.text = "Wrong! Correct answer: " + correctAnswer;
            feedbackText.color = Color.red;
        }

        answerButton1.interactable = false;
        answerButton2.interactable = false;
        answerButton3.interactable = false;

        nextButton.gameObject.SetActive(true);
    }

    public void NextQuestion()
    {
        currentQuestionIndex++;
        ShowQuestion();
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score;
    }

    void ShowFinalResult()
    {
        categoryText.text = "Quiz Finished";
        questionText.text = "Quiz Completed!";
        feedbackText.text = "Final Score: " + score + " / " + questions.Length;
        feedbackText.color = Color.black;

        answerButton1.gameObject.SetActive(false);
        answerButton2.gameObject.SetActive(false);
        answerButton3.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);

        restartButton.gameObject.SetActive(true);
        menuButton.gameObject.SetActive(true);
    }

    public void RestartQuiz()
    {
        currentQuestionIndex = 0;
        score = 0;
        answered = false;

        UpdateScoreUI();
        feedbackText.text = "";

        restartButton.gameObject.SetActive(false);
        menuButton.gameObject.SetActive(false);

        ShowQuestion();
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
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

    void Awake()
    {
        questions = new QuestionData[]
        {
            // HEART ANATOMY
            new QuestionData
            {
                category = "Heart Quiz",
                question = "What is the function of the left ventricle?",
                answers = new string[]
                {
                    "Receives blood from the lungs",
                    "Pumps oxygenated blood to the entire body",
                    "Carries blood to the right atrium"
                },
                correctAnswerIndex = 1
            },
            new QuestionData
            {
                category = "Heart Quiz",
                question = "Which vessel carries deoxygenated blood from the heart to the lungs?",
                answers = new string[]
                {
                    "Aorta",
                    "Pulmonary Vein",
                    "Pulmonary Artery"
                },
                correctAnswerIndex = 2
            },
            new QuestionData
            {
                category = "Heart Quiz",
                question = "The right atrium receives blood from which vessel?",
                answers = new string[]
                {
                    "Aorta",
                    "Vena Cava",
                    "Pulmonary Artery"
                },
                correctAnswerIndex = 1
            },
            new QuestionData
            {
                category = "Heart Quiz",
                question = "What does the aorta do?",
                answers = new string[]
                {
                    "Carries deoxygenated blood to the lungs",
                    "Carries oxygenated blood from the left ventricle to the body",
                    "Connects the right atrium to the right ventricle"
                },
                correctAnswerIndex = 1
            },
            new QuestionData
            {
                category = "Heart Quiz",
                question = "How many chambers does the human heart have?",
                answers = new string[]
                {
                    "2",
                    "3",
                    "4"
                },
                correctAnswerIndex = 2
            },

            // ARRHYTHMIA
            new QuestionData
            {
                category = "Arrhythmia Quiz",
                question = "What is arrhythmia?",
                answers = new string[]
                {
                    "A blockage in the aorta",
                    "An irregular heartbeat caused by abnormal electrical conduction",
                    "Inflammation of the heart muscle"
                },
                correctAnswerIndex = 1
            },
            new QuestionData
            {
                category = "Arrhythmia Quiz",
                question = "Which symptom is most commonly associated with arrhythmia?",
                answers = new string[]
                {
                    "Blurred vision",
                    "Palpitations and dizziness",
                    "Joint pain"
                },
                correctAnswerIndex = 1
            },
            new QuestionData
            {
                category = "Arrhythmia Quiz",
                question = "What happens to blood flow during arrhythmia?",
                answers = new string[]
                {
                    "Blood flow increases significantly",
                    "Blood flow is unaffected",
                    "Irregular electrical conduction may disrupt normal blood flow"
                },
                correctAnswerIndex = 2
            },
            new QuestionData
            {
                category = "Arrhythmia Quiz",
                question = "Which part of the heart controls the electrical signal that triggers each heartbeat?",
                answers = new string[]
                {
                    "Left ventricle",
                    "Sinoatrial (SA) node",
                    "Pulmonary artery"
                },
                correctAnswerIndex = 1
            },

            // VENIPUNCTURE
            new QuestionData
            {
                category = "Venipuncture Quiz",
                question = "What is the preferred vein for venipuncture?",
                answers = new string[]
                {
                    "Cephalic vein",
                    "Median cubital vein",
                    "Basilic vein"
                },
                correctAnswerIndex = 1
            },
            new QuestionData
            {
                category = "Venipuncture Quiz",
                question = "Why is the median cubital vein preferred?",
                answers = new string[]
                {
                    "It is deeper and harder to reach",
                    "It is large, stable, and has less risk of injury",
                    "It carries oxygenated blood only"
                },
                correctAnswerIndex = 1
            },
            new QuestionData
            {
                category = "Venipuncture Quiz",
                question = "What should you do before inserting a needle?",
                answers = new string[]
                {
                    "Shake the patient's arm",
                    "Clean the site with antiseptic",
                    "Apply pressure immediately"
                },
                correctAnswerIndex = 1
            },
            new QuestionData
            {
                category = "Venipuncture Quiz",
                question = "What angle is typically used for venipuncture?",
                answers = new string[]
                {
                    "90 degrees",
                    "45 degrees",
                    "15 to 30 degrees"
                },
                correctAnswerIndex = 2
            },
            new QuestionData
            {
                category = "Venipuncture Quiz",
                question = "What should you do after removing the needle?",
                answers = new string[]
                {
                    "Leave the site uncovered",
                    "Apply pressure to stop bleeding",
                    "Massage the vein"
                },
                correctAnswerIndex = 1
            },

            // SKELETAL SYSTEM
            new QuestionData
            {
                category = "Skeleton Quiz",
                question = "How many bones are in the adult human body?",
                answers = new string[]
                {
                    "300",
                    "206",
                    "180"
                },
                correctAnswerIndex = 1
            },
            new QuestionData
            {
                category = "Skeleton Quiz",
                question = "What is the longest bone in the human body?",
                answers = new string[]
                {
                    "Tibia",
                    "Femur",
                    "Humerus"
                },
                correctAnswerIndex = 1
            },

            // BONE FRACTURES
            new QuestionData
            {
                category = "Fracture Quiz",
                question = "Which type of fracture involves the bone breaking through the skin?",
                answers = new string[]
                {
                    "Hairline fracture",
                    "Stress fracture",
                    "Compound open fracture"
                },
                correctAnswerIndex = 2
            },
            new QuestionData
            {
                category = "Fracture Quiz",
                question = "What is the term for a fracture where the bone breaks into multiple pieces?",
                answers = new string[]
                {
                    "Comminuted fracture",
                    "Greenstick fracture",
                    "Stress fracture"
                },
                correctAnswerIndex = 0
            },

            // MIXED REVIEW
            new QuestionData
            {
                category = "Heart Quiz",
                question = "Which chamber of the heart pumps blood to the lungs?",
                answers = new string[]
                {
                    "Left atrium",
                    "Left ventricle",
                    "Right ventricle"
                },
                correctAnswerIndex = 2
            },
            new QuestionData
            {
                category = "Arrhythmia Quiz",
                question = "Which of these is a risk factor for developing arrhythmia?",
                answers = new string[]
                {
                    "Regular exercise",
                    "High blood pressure and heart disease",
                    "Drinking water frequently"
                },
                correctAnswerIndex = 1
            },
            new QuestionData
            {
                category = "Skeleton Quiz",
                question = "What is the main function of the rib cage?",
                answers = new string[]
                {
                    "To produce hormones",
                    "To protect the heart and lungs",
                    "To connect the skull to the spine"
                },
                correctAnswerIndex = 1
            },
            new QuestionData
            {
                category = "Fracture Quiz",
                question = "What should be done first when a fracture is suspected?",
                answers = new string[]
                {
                    "Move the limb to test it",
                    "Immobilize the injured area",
                    "Massage the painful area"
                },
                correctAnswerIndex = 1
            }
        };
    }

    void ShuffleQuestions()
    {
        for (int i = questions.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            QuestionData temp = questions[i];
            questions[i] = questions[randomIndex];
            questions[randomIndex] = temp;
        }
    }

    void Start()
    {
        nextButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        menuButton.gameObject.SetActive(false);

        feedbackText.text = "";
        ShuffleQuestions();
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
        feedbackText.color = Color.white;

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

        ShuffleQuestions();
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
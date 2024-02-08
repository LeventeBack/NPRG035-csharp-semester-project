# User Documentation

## Introduction - What is QuizEdu?

QuizEdu is a web application that allows teachers to create quizzes for their students. The application is designed to be used in a classroom setting, where the teacher can create a quiz and have a selected student answer the question in front of the class.

## Pages and Features

### 1. Main menu

[![Main menu](./images/1-main-menu.png)](./images/1-main-menu.png)

The main menu of the application contains the following options:

- **_Start Quiz_** - This option allows the user to start a quiz game. The user can select a quiz from the list of existing quizzes and start the game.

- **_Manage Quiz_** - This option allows the user to manage the quizzes. The user can create a new quiz or edit an existing one.

- **_History_** - This option allows the user to view the history of the games played by every user.

### 2. Manage Quiz

[![Manage Quiz](./images/2-manage-quizzes.png)](./images/2-manage-quizzes.png)

- The **_Manage Quiz_** page contains the list of existing quizzes and the option to create a new quiz or edit an existing one.
- The **_Back_** button navigates the user back to the main menu.
- The **_Create New _** button navigates the user to the **_Manage Question_** page where the user can create a new quiz.
- The **_Combine Existing_** button navigates the user to the **_Combine Existing_** page where the user can combine existing quizzes.
- The **_Edit_** button navigates the user to the **_Edit Quiz_** page where the user can edit an existing quiz.
- The **_Delete_** button deletes the selected quiz.

### 3. Create New Quiz

[![Create New Quiz](./images/3-create-quiz.png)](./images/3-create-quiz.png)

- The **_Create New Quiz_** page contains the form for creating a new quiz. The user can enter the name of the quiz and add the questions one by one.
- The round count can also be set as a number. This will be the number of questions in the game.
- After the user filled out the form, the **_Create Question_** button will create the quiz display in the list below the form. The user can add as many questions as they want.
- When adding the question the first answer is the correct one and the rest are wrong. This is also indicated by the green and red colors of the input fields.

### 4. Edit Quiz

[![Edit Quiz](./images/4-edit-quiz.png)](./images/4-edit-quiz.png)

- The **_Edit Quiz_** page contains the form for editing an existing quiz. The user can edit the name of the quiz, the round count and the questions one by one.
- The user can click to the edit button and the respective question will be displayed in the form. The user can edit the question and the answers. After the user is done editing the question, the **_Save Question_** button will save the changes and the updated question will be displayed in the list below the form.
- The user can also delete a question by clicking on the delete button next to each question.

### 5. Combine Existing Quiz

[![Combine Existing](./images/4-combine-quiz.png)](./images/5-combine-quiz.png)

- The **_Combine Existing_** page contains the form for combining existing quizzes. The user can select the quizzes from the list and combine them into a new quiz.
- The user can select the quizzes from the list and add them to the combined quiz. The user can also remove the quizzes from the combined quiz.

### 6. Select Game

[![Select Game](./images/5-select-game.png)](./images/5-select-game.png)

- The **_Select Game_** page contains the list of existing quizzes. The user can select a quiz and start the game.
- This list only contains the quizzes that meet the round count requirement. The user can only select a quiz if the round count is greater than 0.
- The **_Back_** button navigates the user back to the main menu.

### 7. Quiz Game

[![Quiz Elements](./images/6-quiz-elements.png)](./images/6-quiz-elements.png)

- After starting the game, the user will be navigated to the **_Quiz Elements_** page. This page contains the question and the 4 answers.
- There is also a **_score counter_** in the top right corner of the page which indicates the number of correct answers and the total number of questions.
- During the game the title of the quiz is always displayed in the top left corner of the page. Here the user can also see the current round number.

#### Correct Answer

[![Correct Answer](./images/7-correct-answer.png)](./images/7-correct-answer.png)

- When the user selects the correct answer, the answer will be highlighted with green color and the score counter will be updated.
- There is also a sound effect indicating that the answer was correct.

#### Wrong Answer

[![Wrong Answer](./images/8-wrong-answer.png)](./images/8-wrong-answer.png)

- When the user selects the wrong answer, the answer will be highlighted with red color and the score counter will be updated.
- There is also a sound effect indicating that the answer was wrong.
- The correct answer will be highlighted with green color.
- In this case the score counter will not be updated.

#### Endscreen

[![Endscreen](./images/9-endscreen.png)](./images/9-endscreen.png)

- After the user answered all the questions, the **_Endscreen_** page will be displayed.
- This page contains the final score and input fields for the username of the player.
- The **_Save Result and Play Again_** button will navigate the user back to the **_Select Game_** page after saving the result.

### 8. History

[![History](./images/10-history.png)](./images/10-history.png)

- The **_History_** page contains the history of the games played.
- The user can see the date, the name of the quiz, the score and the username of the player.

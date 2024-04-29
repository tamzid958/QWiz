# Question Bank Management System

Welcome to the Question Bank Management System! This system helps manage the creation, review, and categorization of questions for various purposes.

## Table of Contents

- [Introduction](#introduction)
- [Features](#features)
- [Usage](#usage)
  - [Question Bank](#question-bank)
  - [Adding Questions](#adding-questions)
  - [Reviewing Questions](#reviewing-questions)
  - [Managing Categories](#managing-categories)
  - [User Management](#user-management)
- [Installation](#installation)
- [Running the Project Locally](#running-the-project-locally)
- [Accessing Qwiz Website](#accessing-qwiz-website)
- [Future Goals](#future-goals)
- [Contributing](#contributing)
- [License](#license)

## Introduction

This system provides a comprehensive platform for managing questions, categories, and users. Whether you're a question setter, reviewer, or administrator, this system streamlines the process of creating and reviewing questions, ensuring quality and organization.

## Features

- **Question Bank**: Access a centralized repository of approved and rejected questions.
- **Category Management**: Organize questions into categories for easy navigation.
- **User Management**: Admins can create, edit, and block users with specified roles.
- **Review System**: Efficiently review and approve questions submitted by setters.
- **Intuitive Interface**: Easy-to-use interface for seamless navigation and operation.

## Usage

### Question Bank

Once a question has been reviewed by all reviewers and admin, it will appear in the question bank section. To view which questions are already in the question bank, activate "Added". To see rejected questions, switch back to "Rejected".

### Adding Questions

Navigate to the question tab and click the create button to add a new question. Questions automatically enter the review phase upon creation. Edit questions before reviewers' approval if necessary. Once a question enters the question bank, it cannot be deleted.

### Reviewing Questions

Review questions submitted by setters under the assigned category. Choose to approve or reject questions and optionally add comments. View reviews from other reviewers alongside the question.

### Managing Categories

Create categories from the category tab. Deleting a category automatically creates an uncategorized category. Questions under deleted categories move to the uncategorized category.

### User Management

Admins can create users with specified roles and edit user details at any time. Block users to revoke their access.

## Running the Project Locally

To run the project, make sure ports 3000 and 54321 are free and docker must be installed, then execute the following command:

```bash
git clone https://github.com/tamzid958/QWiz.git &&
cd Qwiz &&
docker-compose up -d
```

## Accessing Qwiz Website

Once all services are up and running smoothly in Docker, you can easily access the Qwiz Website by navigating to [http://127.0.0.1:3000](http://127.0.0.1:3000)

### Default Admin Credentials:

- **Username:** tamzid
- **Password:** tamzid

## Future Goals

- **Enhanced Question Editor**: Upgrade your question creation experience with advanced features like rich text formatting, image uploading, and equation rendering.

- **Question Versioning**: Track revisions effortlessly with version control for questions. Easily compare different versions, enhancing collaboration and ensuring transparency in the review process.

- **Integration with Learning Management Systems (LMS)**: Seamlessly import/export questions and synchronize data with popular LMS platforms like Moodle, Canvas, or Blackboard.

- **Automated Question Generation**: Save time and effort by exploring algorithms or plugins for automatically generating questions based on predefined criteria or learning objectives.

- **Analytics Dashboard**: Gain valuable insights into question usage, review progress, user activity, and performance metrics with a comprehensive analytics dashboard.

- **Mobile Application**: Stay connected and productive on the go with our mobile application companion. Access and interact with the platform anytime, anywhere, enhancing accessibility and convenience.

- **Localization and Internationalization**: Embrace diversity and inclusivity with localization and internationalization features. Support multiple languages and cultural preferences to broaden the system's reach.

- **Gamification Elements**: Inject fun and motivation into the learning experience with gamification elements like badges, leaderboards, and rewards.

- **Advanced User Permissions**: Tailor permissions to specific roles and responsibilities with enhanced user management capabilities. Enjoy more granular control over access rights for effective administration.

- **Machine Learning for Content Recommendations**: Personalize the learning journey with machine learning algorithms analyzing user behavior and preferences. Receive tailored content recommendations for questions, categories, and learning resources.

These future goals are designed to elevate the functionality, usability, and effectiveness of the Question Bank Management System, empowering educators, content creators, and learners alike. Let's build a brighter future together! 🚀

## Contributing

Contributions are welcome! If you have any suggestions, feature requests, or bug reports, please submit an issue or create a pull request.

## License

This project is licensed under the [MIT License](LICENSE).

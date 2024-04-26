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
- [Running the Project](#running-the-project)
- [Contributing](#contributing)
- [License](#license)

## Introduction

This system provides a comprehensive platform for managing questions, categories, and users. Whether you're a question setter, reviewer, or administrator, this system streamlines the process of creating and reviewing questions, ensuring quality and organization.

## Features

- **Question Bank**: Access a centralized repository of approved questions.
- **Category Management**: Organize questions into categories for easy navigation.
- **User Management**: Admins can create, edit, and block users with specified roles.
- **Review System**: Efficiently review and approve questions submitted by setters.
- **Intuitive Interface**: Easy-to-use interface for seamless navigation and operation.

## Usage

### Question Bank

Activate "Bank Mode" to view approved questions in the question bank. Easily switch between "Bank Mode" and "Review Mode" to see reviewed or under review questions respectively.

### Adding Questions

Navigate to the question tab and click the create button to add a new question. Questions automatically enter the review phase upon creation. Edit questions before reviewers' approval if necessary. Once a question enters the question bank, it cannot be deleted.

### Reviewing Questions

Review questions submitted by setters under the assigned category. Choose to approve or reject questions and optionally add comments. View reviews from other reviewers alongside the question.

### Managing Categories

Create categories from the category tab. Deleting a category automatically creates an uncategorized category. Questions under deleted categories move to the uncategorized category.

### User Management

Admins can create users with specified roles and edit user details at any time. Block users to revoke their access.

## Installation

To use the Question Bank Management System locally, follow these steps:
1. Clone this repository.
2. Set up the necessary dependencies.
3. Configure the system according to your requirements.

## Running the Project

To run the project, make sure ports 3000 and 65415 are free, then execute the following command:
```bash
docker-compose up -d
```

## Contributing

Contributions are welcome! If you have any suggestions, feature requests, or bug reports, please submit an issue or create a pull request.

## License

This project is licensed under the [MIT License](LICENSE).

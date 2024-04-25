import {
  Category,
  Collections,
  Dashboard,
  QuestionAnswer,
  ReviewsSharp,
  SupervisedUserCircle,
} from "@mui/icons-material";

const navigationLinks = [
  {
    title: "Dashboard",
    icon: <Dashboard />,
    url: "/dashboard",
    access: ["Admin", "Reviewer", "QuestionSetter"],
    hidden: false,
  },
  {
    title: "Question Bank",
    icon: <Collections />,
    url: "/question-bank",
    access: ["Admin"],
    hidden: false,
    description: `Once a question has been reviewed by all reviewers, 
    it will appear in the question bank section. To view which questions 
    are already in the question bank, activate "Bank Mode". To see 
    reviewed questions, switch back to "Review Mode".`,
  },
  {
    title: "Question",
    icon: <QuestionAnswer />,
    url: "/questions",
    access: ["QuestionSetter", "Admin"],
    hidden: false,
    description: `To add a question, navigate to the question tab and 
    click the create button. Once the question is created, it will 
    automatically enter the review phase. If any reviewer has already 
    reviewed the question, it cannot be edited. Before any reviewer 
    reviews it, quickly edit the question if necessary. Once a question 
    is added to the question bank, it cannot be deleted. Press the check 
    button for each question to monitor the review progress.`,
  },
  {
    title: "Review",
    icon: <ReviewsSharp />,
    url: "/reviews",
    access: ["Reviewer", "Admin"],
    hidden: false,
    description: `When a question setter submits a question, it will appear 
    here automatically if the question's category is assigned to you for review. 
    The category accordion will be displayed. Click to expand the category, and 
    the questions listed under each category. Click the view button for the question. 
    Then, if you wish to approve or reject the question, simply choose the respective 
    button and optionally add a comment. You can also view reviews from other reviewers 
    alongside the question. To view already reviewed questions, switch on "Reviewed" mode. 
    To see questions currently under review, switch back to "Under Review" mode.`,
  },
  {
    title: "Category",
    icon: <Category />,
    url: "/categories",
    access: ["Admin"],
    hidden: false,
    description: `To create a category, navigate to the category tab. Then, press 
    the create button and fill out the form. If you delete any category, the system 
    will automatically create an uncategorized category. You can't delete 
    the uncategorized category. Any questions created under the deleted category 
    will be moved to the uncategorized category.`,
  },
  {
    title: "User Management",
    icon: <SupervisedUserCircle />,
    url: "/user-management",
    access: ["Admin"],
    hidden: false,
    description: `Any user can be created from here with role. Random user can't register. 
    Admin should register reviewer or question setter or new admin. If you want to revoke access
    of any user then simply block the user, by switch on blocked section. User can be edited at 
    any time.`,
  },
  {
    title: "Account",
    icon: <SupervisedUserCircle />,
    url: "/account",
    access: ["Admin", "Reviewer", "QuestionSetter"],
    hidden: true,
  },
];

export default navigationLinks;

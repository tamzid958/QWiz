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
  },
  {
    title: "Question",
    icon: <QuestionAnswer />,
    url: "/questions",
    access: ["QuestionSetter", "Admin"],
    hidden: false,
  },
  {
    title: "Review",
    icon: <ReviewsSharp />,
    url: "/reviews",
    access: ["Reviewer", "Admin"],
    hidden: false,
  },
  {
    title: "Category",
    icon: <Category />,
    url: "/categories",
    access: ["Admin"],
    hidden: false,
  },
  {
    title: "User Management",
    icon: <SupervisedUserCircle />,
    url: "/user-management",
    access: ["Admin"],
    hidden: false,
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

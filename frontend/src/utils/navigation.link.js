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
  },
  {
    title: "Question Bank",
    icon: <Collections />,
    url: "/question-bank",
    access: ["Admin"],
  },
  {
    title: "Question",
    icon: <QuestionAnswer />,
    url: "/questions",
    access: ["QuestionSetter", "Admin"],
  },
  {
    title: "Review",
    icon: <ReviewsSharp />,
    url: "/reviews",
    access: ["Reviewer", "Admin"],
  },
  {
    title: "Category",
    icon: <Category />,
    url: "/categories",
    access: ["Admin"],
  },
  {
    title: "User Management",
    icon: <SupervisedUserCircle />,
    url: "/user-management",
    access: ["Admin"],
  },
];

export default navigationLinks;

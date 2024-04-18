import {
  Category,
  Dashboard,
  QuestionAnswer,
  ReviewsSharp,
  SupervisedUserCircle,
} from "@mui/icons-material";

const navigationLinks = [
  {
    title: "Dashboard",
    icon: <Dashboard />,
    url: "/",
    access: ["Admin", "Reviewer", "QuestionSetter"],
  },
  {
    title: "Question",
    icon: <QuestionAnswer />,
    url: "/question",
    access: ["QuestionSetter"],
  },
  {
    title: "Review",
    icon: <ReviewsSharp />,
    url: "/review",
    access: ["Reviewer"],
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

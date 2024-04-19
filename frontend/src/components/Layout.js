"use client";
import * as React from "react";
import { useState } from "react";
import { styled, useTheme } from "@mui/material/styles";
import Box from "@mui/material/Box";
import Drawer from "@mui/material/Drawer";
import MuiAppBar from "@mui/material/AppBar";
import Toolbar from "@mui/material/Toolbar";
import List from "@mui/material/List";
import Divider from "@mui/material/Divider";
import IconButton from "@mui/material/IconButton";
import MenuIcon from "@mui/icons-material/Menu";
import ChevronLeftIcon from "@mui/icons-material/ChevronLeft";
import ListItem from "@mui/material/ListItem";
import ListItemButton from "@mui/material/ListItemButton";
import ListItemIcon from "@mui/material/ListItemIcon";
import ListItemText from "@mui/material/ListItemText";
import navigationLinks from "@/utils/navigation.link";
import { usePathname, useRouter } from "next/navigation";
import {
  Avatar,
  Breadcrumbs,
  Fab,
  Link,
  Paper,
  Tooltip,
  Typography,
} from "@mui/material";
import { useSession } from "next-auth/react";
import {
  breadCrumbGenerator,
  getLettersFromString,
  textToDarkLightColor,
} from "@/utils/common";
import _ from "lodash";
import { ToastContainer } from "react-toastify";
import Logo from "../../public/logo.png";
import Image from "next/image";

const drawerWidth = 240;

const Main = styled("main", { shouldForwardProp: (prop) => prop !== "open" })(
  ({ theme, open }) => ({
    flexGrow: 1,
    padding: theme.spacing(3),
    transition: theme.transitions.create("margin", {
      easing: theme.transitions.easing.sharp,
      duration: theme.transitions.duration.leavingScreen,
    }),
    marginLeft: `-${drawerWidth}px`,
    ...(open && {
      transition: theme.transitions.create("margin", {
        easing: theme.transitions.easing.easeOut,
        duration: theme.transitions.duration.enteringScreen,
      }),
      marginLeft: 0,
    }),
  }),
);

const AppBar = styled(MuiAppBar, {
  shouldForwardProp: (prop) => prop !== "open",
})(({ theme, open }) => ({
  transition: theme.transitions.create(["margin", "width"], {
    easing: theme.transitions.easing.sharp,
    duration: theme.transitions.duration.leavingScreen,
  }),
  ...(open && {
    width: `calc(100% - ${drawerWidth}px)`,
    marginLeft: `${drawerWidth}px`,
    transition: theme.transitions.create(["margin", "width"], {
      easing: theme.transitions.easing.easeOut,
      duration: theme.transitions.duration.enteringScreen,
    }),
  }),
}));

const DrawerHeader = styled("div")(({ theme }) => ({
  display: "flex",
  alignItems: "center",
  padding: theme.spacing(0, 1),
  // necessary for content to be below app bar
  ...theme.mixins.toolbar,
  justifyContent: "flex-end",
}));

const Layout = ({ children }) => {
  const theme = useTheme();
  const [openDrawer, setOpenDrawerDrawer] = useState(false);

  const pathname = usePathname();
  const router = useRouter();
  const session = useSession();

  const handleDrawerOpen = () => setOpenDrawerDrawer(true);

  const handleDrawerClose = () => setOpenDrawerDrawer(false);

  const userFullName = session?.data?.user?.fullName;
  const roles = session?.data?.user?.roles ?? [];

  if (pathname.startsWith("/auth")) {
    return <>{children}</>;
  }

  return (
    <Box sx={{ display: "flex" }}>
      <AppBar
        position="fixed"
        open={openDrawer}
        className="bg-gradient-to-r from-black to-gray-900"
      >
        <Toolbar className="flex justify-between">
          <IconButton
            color="inherit"
            aria-label="open drawer"
            onClick={handleDrawerOpen}
            edge="start"
            sx={{ mr: 2, ...(openDrawer && { display: "none" }) }}
          >
            <MenuIcon />
          </IconButton>
          <Tooltip title="Account settings">
            <IconButton
              onClick={() => {
                router.push("/account");
              }}
              size="large"
              sx={{ ml: 2 }}
            >
              <div className="flex items-center gap-3">
                <Avatar
                  sx={{ width: 32, height: 32 }}
                  style={{
                    backgroundColor: textToDarkLightColor(userFullName ?? "")
                      .darkColor,
                  }}
                >
                  <Typography className="text-white font-bold font-sm">
                    {getLettersFromString(userFullName ?? "N/A")}
                  </Typography>
                </Avatar>
                <Typography className="text-white font-bold">
                  {userFullName}
                </Typography>
              </div>
            </IconButton>
          </Tooltip>
        </Toolbar>
      </AppBar>
      <Drawer
        sx={{
          width: drawerWidth,
          flexShrink: 0,
          "& .MuiDrawer-paper": {
            width: drawerWidth,
            boxSizing: "border-box",
          },
        }}
        variant="persistent"
        anchor="left"
        open={openDrawer}
      >
        <DrawerHeader>
          <div className="flex justify-between gap-x-36">
            <Image src={Logo} alt="logo" className="w-10 h-10" />
            <IconButton onClick={handleDrawerClose}>
              <ChevronLeftIcon />
            </IconButton>
          </div>
        </DrawerHeader>
        <Divider />
        <List>
          {navigationLinks
            .filter((o) => _.intersection(o.access, roles).length > 0)
            .map((o) => (
              <ListItem
                key={o.title}
                disablePadding
                className={pathname === o.url ? "bg-gray-200" : ""}
              >
                <ListItemButton
                  onClick={() => {
                    router.push(o.url);
                    handleDrawerClose();
                  }}
                >
                  <ListItemIcon>{o.icon}</ListItemIcon>
                  <ListItemText primary={o.title} />
                </ListItemButton>
              </ListItem>
            ))}
        </List>
      </Drawer>
      <Main
        open={openDrawer}
        className="h-screen bg-gradient-to-br from-gray-100 to-gray-300"
      >
        <DrawerHeader />
        <ToastContainer />
        <div
          className={`${_.includes(pathname, "create") || _.includes(pathname, "update") ? "w-2/4 mx-auto" : "w-full items-start justify-center"} `}
        >
          <Paper elevation={1} className="p-4 flex flex-col gap-5">
            <div className="flex justify-between items-center flex-wrap gap-4">
              <Breadcrumbs separator="›" aria-label="breadcrumb">
                {breadCrumbGenerator(pathname).map((p) => (
                  <Link
                    underline="hover"
                    className="cursor-pointer"
                    key="1"
                    color="inherit"
                    onClick={() => router.push(p.path)}
                  >
                    {p.name}
                  </Link>
                ))}
              </Breadcrumbs>

              {children}
            </div>
          </Paper>
        </div>
        <Tooltip title="Developed by Tamzid">
          <Fab
            aria-label="add"
            className={"absolute bottom-5 right-5 bg-black"}
          >
            <Image src={Logo} alt="logo" className="w-10 h-10" />
          </Fab>
        </Tooltip>
      </Main>
    </Box>
  );
};
export default Layout;

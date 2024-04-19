import _ from "lodash";
import dateFormat from "dateformat";

const getLettersFromString = (str) => {
  const parts = str.split(" "); // Split the full name into parts by space
  let initials = "";

  // Iterate over each part to extract the first letter
  parts.forEach((part) => {
    initials += part.charAt(0).toUpperCase(); // Add the first letter of each part, capitalized
  });

  return initials;
};

const textToDarkLightColor = (text) => {
  // Calculate a hash value for the input text
  let hash = 0;
  for (let i = 0; i < text.length; i++) {
    hash = text.charCodeAt(i) + ((hash << 5) - hash);
  }

  // Generate dark color
  let darkColor = "#";
  for (let i = 0; i < 3; i++) {
    let value = (hash >> (i * 8)) & 0xff;
    value = Math.min(150, value); // Ensure darkness
    darkColor += ("00" + value.toString(16)).substr(-2);
  }

  // Generate light color (invert dark color)
  let lightColor = "#";
  for (let i = 0; i < 3; i++) {
    let value = parseInt(darkColor.substr(1 + i * 2, 2), 16);
    value = 255 - value;
    lightColor += ("00" + value.toString(16)).substr(-2);
  }

  return { dark: darkColor, light: lightColor };
};

const breadCrumbGenerator = (pathName) => {
  // Decode URL encoded characters and split the pathName into individual segments
  const decodedPath = decodeURIComponent(pathName);
  const paths = decodedPath
    .split("/")
    .filter((segment) => segment.trim() !== "");

  // Initialize an empty array to store the breadcrumbs
  let breadcrumbs = [];

  // Iterate through each segment of the path
  let currentPath = "";
  paths.forEach((segment) => {
    // Capitalize the first letter of each word in the segment name
    const segmentName = segment
      .replace(/\b\w/g, (c) => c.toUpperCase())
      .replace("-", " ");

    // Construct the path up to the current segment
    currentPath += "/" + segment;

    // Construct the breadcrumb object for the current segment
    const breadcrumb = {
      name: segmentName,
      path: currentPath,
    };

    // Add the breadcrumb to the array
    breadcrumbs.push(breadcrumb);
  });

  return forUpdatePaths([{ name: "Dashboard", path: "/" }].concat(breadcrumbs));
};

function forUpdatePaths(items) {
  const updatedBreadCrumb = [];
  let removeObjectPath = null;
  _.forEach(items, (item) => {
    if (_.includes(item.name, "Update")) {
      removeObjectPath = getNextObject(items, item).path;
    }
    if (item.path !== removeObjectPath) {
      updatedBreadCrumb.push({
        name: item.name,
        path: _.includes(item.name, "Update")
          ? getNextObject(items, item)?.path
          : item.path,
      });
    }
  });

  return updatedBreadCrumb;
}

function getNextObject(objects, currentObject) {
  const index = _.findIndex(objects, currentObject);
  if (index !== -1 && index < objects.length - 1) {
    return objects[index + 1];
  }
  return null;
}

const formatDate = (date) => {
  return dateFormat(new Date(date), "dddd, mmmm dS, yyyy, h:MM TT");
};

export {
  getLettersFromString,
  textToDarkLightColor,
  breadCrumbGenerator,
  formatDate,
};

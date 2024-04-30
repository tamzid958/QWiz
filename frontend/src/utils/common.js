import _ from "lodash";
import dateFormat from "dateformat";

const getLettersFromString = (str, limit = null) => {
  const parts = str.split(" "); // Split the full name into parts by space
  let initials = "";

  // Iterate over each part to extract the first letter
  parts.forEach((part) => {
    initials += part.charAt(0).toUpperCase(); // Add the first letter of each part, capitalized
  });

  return limit ? initials.substring(0, limit) : initials;
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

  return forUpdatePaths(breadcrumbs);
};

function forUpdatePaths(items) {
  const updatedBreadCrumb = [];
  let removeObjectPath = null;
  _.forEach(items, (item) => {
    if (_.includes(item.name, "Update") || _.includes(item.name, "View")) {
      removeObjectPath = getNextObject(items, item).path;
    }
    if (item.path !== removeObjectPath) {
      updatedBreadCrumb.push({
        name: item.name,
        path:
          _.includes(item.name, "Update") || _.includes(item.name, "View")
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
  return dateFormat(new Date(date), "mmmm dS, yyyy, h:MM TT");
};

function sortByBooleanProperty(objects, propertyName) {
  const order = {
    true: 1,
    false: 2,
    null: 3,
  };

  const getNestedPropertyValue = (obj, keys) => {
    if (!obj || !keys.length) return null;
    const [currentKey, ...remainingKeys] = keys;
    const nextObj = obj[currentKey];
    return remainingKeys.length
      ? getNestedPropertyValue(nextObj, remainingKeys)
      : nextObj;
  };

  return objects.sort((a, b) => {
    const aValue = getNestedPropertyValue(a, propertyName.split("."));
    const bValue = getNestedPropertyValue(b, propertyName.split("."));

    return (order[aValue] || 0) - (order[bValue] || 0);
  });
}

const createReviewersWithLog = (reviewers, reviewLogs) => {
  if (reviewers?.length === 0 && reviewLogs?.length > 0) {
    return sortByBooleanProperty(
      reviewLogs.map((o) => ({
        id: o.id,
        appUserId: o.createdById,
        fullName: o.createdBy.fullName,
        log: o,
      })),
      "review.log.isApproved",
    );
  }

  return reviewers && reviewLogs
    ? sortByBooleanProperty(
        reviewers.map((o) => ({
          id: o.id,
          appUserId: o.appUserId,
          fullName: o.appUser.fullName,
          log: reviewLogs.find((x) => x.createdById === o.appUserId) ?? null,
        })),
        "review.log.isApproved",
      )
    : [];
};

const parseJwt = (token) =>
  JSON.parse(Buffer.from(token.split(".")[1], "base64").toString());

export {
  getLettersFromString,
  textToDarkLightColor,
  breadCrumbGenerator,
  formatDate,
  sortByBooleanProperty,
  createReviewersWithLog,
  parseJwt,
};

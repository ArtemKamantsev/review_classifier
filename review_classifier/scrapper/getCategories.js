'use strict';

const gPlay = require('google-play-scraper');
const utils = require('./utils');

const getCategories = () => {
  try {
    const categories = Object.values(gPlay.category);

    return utils.returnValue(categories);
  } catch (e) {
    return utils.returnValue([], `${e}`);
  }
};

getCategories();

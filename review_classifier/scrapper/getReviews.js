'use strict';

const gPlay = require('google-play-scraper');
const utils = require('./utils');

const getReviews = async () => {
  try {
    const parameters = utils.getInputParams();
    const category = parameters[0];
    const categories = category ? [category] : [];

    const timeLimit = Number(parameters[1]);
    const timeFinish = Date.now() + timeLimit;

    const appIds = await utils.getAppIds(categories, Object.values(gPlay.collection).slice(0, 1));
    const reviews = await utils.getReviews(appIds, timeFinish);

    return utils.returnValue(reviews);
  } catch (e) {
    return utils.returnValue([], `${e}`);
  }
};

getReviews();

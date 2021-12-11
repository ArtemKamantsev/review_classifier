'use strict';

const gPlay = require('google-play-scraper');
const utils = require('./utils');

const getApps = async () => {
  try {
    const parameters = utils.getInputParams();
    const category = parameters[0];

    const apps = await utils.getAppNames([category], Object.values(gPlay.collection).slice(0, 1));
    return utils.returnValue(apps);
  } catch (e) {
    return utils.returnValue([], `${e}`);
  }
};

getApps();

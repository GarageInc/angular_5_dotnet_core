import 'reflect-metadata';
import 'zone.js/dist/zone-node';
import { platformServer, renderModuleFactory } from '@angular/platform-server';
import { enableProdMode } from '@angular/core';
// * NOTE :: leave this as require() since this file is built Dynamically from webpack
const {
  AppServerModuleNgFactory,
  LAZY_MODULE_MAP
} = require('./../../wwwroot/server/main.bundle');
// import { AppServerModuleNgFactory } from './../../wwwroot/server/main.bundle';
import * as express from 'express';
import { readFileSync } from 'fs';
import { join } from 'path';

// Express Engine
import { ngExpressEngine } from '@nguniversal/express-engine';
// Import module map for lazy loading
import { provideModuleMap } from '@nguniversal/module-map-ngfactory-loader';

const PORT = 4001;

enableProdMode();

const app = express();
const DIST_FOLDER = join(process.cwd(), 'wwwroot');

const template = readFileSync(
  join(DIST_FOLDER, 'search_module', 'index.html')
).toString();

app.engine(
  'html',
  ngExpressEngine({
    bootstrap: AppServerModuleNgFactory,
    providers: [provideModuleMap(LAZY_MODULE_MAP)]
  })
);

app.set('view engine', 'html');
app.set('views', 'wwwroot/search_module');

/*
// TODO: implement data requests securely
app.get('/api/*', (req, res) => {
  res.status(404).send('data requests are not supported');
});

/*
app.get(
  '*.*',
  express.static(join(__dirname, '../..', 'wwwroot', 'search_module'))
);

app.get('*', (req, res) => {
  res.render('index', { req });
});
*/

// Server static files from /browser
app.get('*.*', express.static(join(DIST_FOLDER, 'search_module')));

// All regular routes use the Universal engine
app.get('*', (req, res) => {
  res.render(join(DIST_FOLDER, 'search_module', 'index.html'), { req });
});

app.listen(PORT, () => {
  console.log(`listening on http://localhost:${PORT}!`);
});

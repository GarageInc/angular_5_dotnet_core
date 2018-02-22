# DepotApp

This project was generated with [Angular CLI](https://github.com/angular/angular-cli) version 1.5.2.

## Development server

Run `ng serve` for a dev server. Navigate to `http://localhost:4200/`. The app will automatically reload if you change any of the source files.

## Code scaffolding

Run `ng generate component component-name` to generate a new component. You can also use `ng generate directive|pipe|service|class|guard|interface|enum|module`.

## Build

Run `ng build` to build the project. The build artifacts will be stored in the `dist/` directory. Use the `-prod` flag for a production build.

## Running unit tests

Run `ng test` to execute the unit tests via [Karma](https://karma-runner.github.io).

## Running end-to-end tests

Run `ng e2e` to execute the end-to-end tests via [Protractor](http://www.protractortest.org/).

## Further help

To get more help on the Angular CLI use `ng help` or go check out the [Angular CLI README](https://github.com/angular/angular-cli/blob/master/README.md).

Instructions:
https://medium.com/@levifuller/building-an-angular-application-with-asp-net-core-in-visual-studio-2017-visualized-f4b163830eaa

CLI:
https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet

MySQL:
GRANT ALL ON depot.\* TO 'depotuser'@'localhost' IDENTIFIED BY 'depotuser';
SELECT user,host,password FROM mysql.user;

Как получить последние курсы со стороны сервера?
Установите: https://github.com/hay/xml2json
apt-get install python-pip
sudo pip install https://github.com/hay/xml2json/zipball/master
chmod +x xml2json

Добавьте такой код в /etc/cron.hourly/cbr на сервере:
#!/bin/sh
set -e
mkdir -p /var/www/depot-service/files/cbr
cd /var/www/depot-service/files/cbr
wget --timestamping --no-verbose www.cbr.ru/scripts/XML_daily.asp
mv XML_daily.asp XML_daily.xml
sudo xml2json -t xml2json -o XML_daily.json XML_daily.xml
#done 2>&1 | xargs -I{} logger --tag $0 --id=$$ "{}"

Затем дайте права на выполнение:

sudo chmod +x /etc/cron.hourly/cbr
И сделайте первую загрузку:
sudo /etc/cron.hourly/cbr

https://nodejs.org/en/download/package-manager/#debian-and-ubuntu-based-linux-distributions

add deploy to www-data groups
and check with command: sudo -Hu deploy sh -c 'dotnet /var/www/depot-service/bin/Debug/netcoreapp2.0/test.dll'

Добавить в крон CURL /etc/cron.hourly/parser:
#!/bin/sh
curl -i localhost:4000/api/v1/command/run-scheduler
#done 2>&1 | xargs -I{} logger --tag $0 --id=$$ "{}"

Затем дайте права на выполнение:
sudo chmod +x /etc/cron.hourly/parser

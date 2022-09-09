var fs = require('fs');
var path = require('path');
var replaceFile = require('replace-in-file');
var package = require("./package.json");
var angular = require("./angular.json");
var buildVersion = package.version;
var buildPath = '\\';
var defaultProject = angular.defaultProject;
var appendUrl = '?v=' + buildVersion;
const getNestedObject = (nestedObj, pathArr) => {
    return pathArr.reduce((obj, key) =>
        (obj && obj[key] !== 'undefined') ? obj[key] : undefined, nestedObj);
}
const relativePath = getNestedObject(angular, ['projects', defaultProject, 'architect', 'build', 'options', 'outputPath']); // to identify relative build path when angular make build
buildPath += relativePath.replace(/[/]/g, '\\');

var indexPath = __dirname + buildPath + '/' + 'index.html';
indexPath = indexPath.split('\\').join('/');
const ngBuildPath = (__dirname + buildPath).split('\\').join('/');
console.log('Index path:', indexPath);
console.log('Angular build path: ', ngBuildPath);
console.log('Change by buildVersion: ', buildVersion);

try {
    fs.readdir(ngBuildPath, (err, files) => {
        console.log('readdir');
        console.log('ngBuildPath : ', ngBuildPath);
        console.log('files :', files);
        console.log('files json:', JSON.stringify(files));

        files.forEach(file => {

            if (file.match(/^(es2015-polyfills|main|polyfills|runtime|scripts|styles)\.[a-z0-9]+\.(js|css)$/)) { // regex is identified by build files generated
                console.log('Current Filename: ', file);
                const currentPath = file;
                const changePath = file + appendUrl;
                changeIndex(currentPath, changePath);
            }
        });


    });
} catch (error) {
    console.error('Error occurred when change files: ', error);
    throw error
}

function changeIndex(currentfilename, changedfilename) {
    const options = {
        files: indexPath,
        from: '"' + currentfilename + '"',
        to: '"' + changedfilename + '"',
        allowEmptyPaths: false,
    };
    try {
        let changedFiles = replaceFile.sync(options);
        if (changedFiles == 0) {
            console.log("File updated failed");
        } else if (changedFiles[0].hasChanged === false) {
            console.log("File already updated");
        }
        console.log('Changed Filename: ', changedfilename);
    }
    catch (error) {
        console.error('Error occurred: ', error);
        throw error
    }
}
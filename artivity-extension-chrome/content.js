console.log("Sending to Zeitgeist: [", document.URL, ",", document.title, "]");

var request = new XMLHttpRequest();
var url = 'http://localhost:8272/artivity/1.0/activities';
var params = '{"title":"' + document.title + '","url":"' + document.URL + '"}';

console.log(params);

request.open("POST", url, true);
request.setRequestHeader("Content-type", "application/json");
request.send(params);
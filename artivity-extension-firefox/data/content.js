console.log(">>> Sending to Zeitgeist: [", document.URL, ",", document.title, "]");

var request = new XMLHttpRequest();
var url = 'http://localhost:8272/artivity/1.0/activities';
var params = {title: document.title, url: document.URL, actor: "application://firefox.desktop"}

console.log(JSON.stringify(params));

request.open("POST", url, true);
request.setRequestHeader("Content-type", "text/plain");
request.send(JSON.stringify(params));

window.getBrowserInfo = function () {

    //DotNet.invokeMethodAsync('GeoProjectDemo', 'InitJS', "param")
    //    .then(message => {
    //        console.log(message);
    //    });

    return navigator.userAgent;
    //ezt a blazor adja, így lehet bármilyen static ( [JSInvokable] ) fv-t hívni
    //az első paraméter a teljes projekt neve, nem a pontos namespace
    //a második paraméter a függvény neve
    // a további paramétereket a függvény kapja meg
    //DotNet.invokeMethodAsync('BlazorApp1', 'GetHelloMessage', "param")
    //    .then(message => {
    //        console.log(message);
    //    });
}

window.onclose = function () {
    DotNet.invokeMethodAsync('GeoProjectDemo', 'InitJS', "param")
        .then(message => {
            console.log(message);
        });

}

window.saveAsFile = function (fileName, byteBase64) {
    var link = this.document.createElement('a');
    link.download = fileName;
    link.href = "data:application/octet-stream;base64," + byteBase64;
    this.document.body.appendChild(link);
    link.click();
    this.document.body.removeChild(link);
}

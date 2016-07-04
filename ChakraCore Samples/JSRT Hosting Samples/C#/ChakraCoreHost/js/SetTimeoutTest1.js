host.echo("Set Timeout Test 1");
function onTimeout() {
    host.echo("onTimeout called");
}

setTimeout(onTimeout, 3000);
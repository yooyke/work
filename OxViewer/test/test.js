var ctrl0 = null;
var ctrl1 = null;

function init() {
	
	var plugin0 = opvw.Plugin;
	plugin0.CODE_BASE = "<!--__install_codebase__-->";
	plugin0.loadAllAPI();
	
	ctrl0 = plugin0.start('js_object_fix0', 640, 480, eventListener0);
	
	
	var plugin1 = opvw.Plugin;
	plugin1.CODE_BASE = "<!--__install_codebase__-->";
	plugin1.loadAllAPI();
	
	ctrl1 = plugin1.start('js_object_fix1', 320, 240, eventListener1);
}

function login0()
{
	ctrl0.Login(login_first0.value, login_last0.value, login_pass0.value, 'http://210.188.235.171:10001', 'last');
}

function login1()
{
	ctrl1.Login(login_first1.value, login_last1.value, login_pass1.value, 'http://210.188.235.171:10001', 'last');
}

function eventListener0(message)
{
}

function eventListener1(message)
{
}

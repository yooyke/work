/*
 * 3Di OpenViewer JavaScript Library 
 *  
 * Copyright 2009,  3Di,Inc.
 * All rights reserved.
 * 
 * This library provides communication support between 3Di OpenViewer Plug-in and DHTML.
 * The library will work on following environments:
 *  IE 6.0+
 *  Mozilla 3.0+
 *  
 */
/**
 * Loginout class
 * 
 * @class
 */
opvw.API.Loginout = opvw.API.Loginout || {
	
	/**
	 * Login with specified account data.
	 * 
	 * @param {string} firstName
	 * @param {string} lastName
	 * @param {string} password
	 * @param {string} serverUri "http://login-server-uri"
	 * @param {string} loginLocation optional "uri:REGION_NAME&X&Y&Z" or "home" or "last"
	 */
	Login : function( firstName, lastName, password, serverUri, loginLocation ){
		var val = JSON.stringify({"first":firstName, "last":lastName, "password":password, "uri":serverUri, "location":loginLocation});
		var msg = JSON.stringify({"key":"login", "value":val});
		opvw.Plugin.callFunction(msg);
	},

	/**
	 * Logout immediately.
	 */
	Logout : function(){
		var msg = JSON.stringify({"key":"logout"});
		opvw.Plugin.callFunction(msg);
	}
	
};
opvw.APIWrapper["Login"] = opvw.API.Loginout.Login;
opvw.APIWrapper["Logout"] = opvw.API.Loginout.Logout;

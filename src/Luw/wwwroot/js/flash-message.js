/**
 * jQuery Flash Message plugin
 *
 * Copyright (c) 2011 Brandon Calloway
  * Licensed under the MIT license:
 * http://www.opensource.org/licenses/mit-license.php
 **/

/**
 *
 * Provides Flash Message functionality similar to a Ruby on Rails application, after a CRUD operation.
 *
 * For example, in an ASP.NET MVC project, you can place the following after a controller action has completed:
 *
 *  Response.Cookies.Add(new HttpCookie("FlashSuccess", "The page was succesfully created!") { Path = "/" });
 *  return RedirectToAction("Index");
 *
 * This will create a cookie named "FlashSuccess", and will be read by the $.flashMessage jQuery function.
 * Additionally, there is a function called flashPrompt that will allow you to use flash messages in ajax success/error callbacks.
 *
 * Requirements:
 *   jQuery 1.4.x or higher, http://docs.jquery.com/Downloading_jQuery
 *   jQuery cookie plugin, http://plugins.jquery.com/project/Cookie
 *
 * Be sure to include this javascript file after the above requirements, and before any javascript files that use flash messages.
 * 
 * In your main javascript file, include the plugin like so:
 *
 *  $("#messages").flashMessage();
 *
 * By default, the flash message looks for the following div container:
 *
 *   <div id="messages"></div>
 *
 * If the flash message is a success, it appends a class called "flash-success". If the flash message is an error, it appends a class called "flash-error".
 * You can style the flash message however you choose in your css.
 *
 * To use the flashPrompt function to provide an ajax callback:
 *
 *   flashPrompt('succes/error', 'Whatever you want the message to be');
 *
 * For example:
 *
 *   $.ajax({
 *	    url: '/admin/pages/delete',
 *	    type: "POST",
 *	    traditional: true,
 *	    data: { 'id': page_id },
 *	    success: function () {
 *       flashPrompt('success', 'Page was successfully Deleted!');
 *      }
 *   });
 *
 **/

// add flash message after jquery operations
function flashPrompt(result, message) {
  $('#messages').addClass("alert-" + result);
  $('#messages').html('<div>' + message + '</div>').fadeIn("slow");
  $('#messages').delay(10000).fadeOut("slow", function () {
    $('#messages').removeClass("alert-" + result);
  })
}

$(document).ready(function () {
    {
        if (Cookies("FlashSuccess")) {
            var messageText = Cookies.get('FlashSuccess');
            if (messageText != null) {
                var message = "<i class='fa fa-check-square'></i> " + messageText;
                flashPrompt("success", message);
                Cookies.remove("FlashSuccess", { path: '' });
            }
        }
        else if (Cookies("FlashError")) {
            var messageText = Cookies.get('FlashError');
            if (messageText != null) {
                var message = "<i class='fa fa-times-circle'></i> " + messageText;
                flashPrompt("danger", message);
                Cookies.remove("FlashError", { path: '' });
            }
        }
        else if (Cookies("FlashWarning")) {
            var messageText = Cookies.get('FlashWarning');
            if (messageText != null) {
                var message = "<i class='fa fa-exclamation-triangle'></i> " + messageText;
                flashPrompt("warning", message);
                Cookies.remove("FlashWarning", { path: '' });
            }
        }
        else if (Cookies("FlashInfo")) {
            var messageText = Cookies.get('FlashInfo');
            if (messageText != null) {
                var message = "<i class='fa fa-info-circle'></i> " + messageText;
                flashPrompt("info", message);
                Cookies.remove("FlashInfo", { path: '' });
            }
        }
    }
});
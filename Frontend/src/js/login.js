$(function(){

	$('#submitButton').click(function(){
		$('.login-loader-container, .form-container, #cannotLoginLink').toggleClass('hidden');
		setTimeout(function(){
			window.location = "index.html";
		}, 5000);
	});

})
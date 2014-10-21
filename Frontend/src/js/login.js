$(function(){

	// Log in
	$('#submitButton').click(function(){
		$('.login-loader-container').toggleClass('hidden');
		setTimeout(function(){
			window.location = "index.html";
		}, 5000);
	});

	$('#createAccountLink').click(function(){
		// Buscar mejor animacion
		$('.signup-container, .signup-title').fadeIn();
		$('.login-container').fadeOut();
	});

	// Sign up
	$('#createAccount').click(function(){
		$('.sign-up-loader-container').toggleClass('hidden');
		setTimeout(function(){
			window.location = "index.html";
		}, 5000);
	});

	$('#backArrowContainer').click(function(){
		// Buscar mejor animacion
		$('.signup-container, .signup-title').fadeOut();
		$('.login-container').fadeIn();
	});

	$('#browseProfilePicture').click(function(){
		$('#profilePictureFileInput').trigger('click');
	});

	$('#profilePictureFileInput').on('change', function(){
		var file = this.files[0];
		var name = file.name;
		var size = file.size;
		var type = file.type;
		filename = name;
		readURL(this, $('#profilePicture'));
	});

})
$('#optionsList li').click(function(){
	$('#optionsList li').removeClass('selected');
	$(this).addClass('selected');

	$('.option-content:visible').hide();
	var selectedOption = $(this).attr('data-option-id');
	$(selectedOption).show().addClass('animated fadeInRight');
});

// Browse for profile picture
$('#browseProfilePicture').click(function(){
	$('#profilePictureFileInput').trigger('click');
});

// File input on change
$('#profilePictureFileInput').on('change', function(){
	var file = this.files[0];
	var name = file.name;
	var size = file.size;
	var type = file.type;
	console.log("file",file);
	filename = name;
	console.log("asdasd", filename);
	readURL(this, $('#profilePicture'));
});
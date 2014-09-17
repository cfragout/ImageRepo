function addToImageBoard(image) {
	$('.image-board').append('<div class="image-container">' +
							 '<img src="'+ image.path +'" alt="' + image.name +'">' +
							 '</div>');
}

function loadImages() {
	$.get('http://localhost:53079/api/Imagenes', function( data ) {
		$.each(data, function(index, image){
			addToImageBoard(image);
		});
	});
}

function getSelectedImagesHTML() {
	return $('.image-container.selected');
}

function getImageFromImageContainer(imageContainer) {
	return $(imageContainer).find('img')[0];
}

$(function(){

	loadImages();

	$('.image-board').on('click', '.image-container', function(){
		$(this).toggleClass('selected');


		var selectedCount = getSelectedImagesHTML().length;
		if (selectedCount == 1) {
			$('.actions span').removeClass('fg-gray no-hover');
			$('.actions').removeClass('no-hover');
		} else if (selectedCount > 1) {
			$('.actions .icon-link').addClass('fg-gray no-hover');
			$('.actions span.icon-link').closest('.actions').addClass('no-hover');
		} else {
			$('.actions span').addClass('fg-gray no-hover');
			$('.actions').addClass('no-hover');
		}

	});

	$('.icon-link').parent().click(function(){
		var link = $(this);
		var selectedImages = getSelectedImagesHTML();

		if ((selectedImages.length == 0) ||(selectedImages.length > 1)) {
			link.removeAttr('href');
			return;
		}

		var imageHTML = getImageFromImageContainer(selectedImages[0]);
		link.attr('href', imageHTML.src);
	});
});
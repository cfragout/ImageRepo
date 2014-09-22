var baseURL = 'http://localhost:53079/Api/';
var postImage = baseURL + 'Imagen/PostImagen';
var getImage = baseURL + 'Imagen/';

function createImageElement(path, name) {
	var imageElement = $('<div class="image-container image-element"></div>');
	imageElement.append('<img src="'+ path +'" alt="' + name +'">');

	return imageElement;
};

function addImageToBoard(image) {
	var image = createImageElement(image.path, image.name);
	var $container = $('#image-board');
	$container.isotope('insert', image)
}

function addImageArrayToBoard(imagesObjArray) {
	var $container = $('#image-board');
	var imageArray = [];

	$.each(imagesObjArray, function(index, image){
		imageArray.push(createImageElement(image.path, image.name)[0]);
	});

	$container.isotope('insert', imageArray)
}

function loadImages() {
	$.get(getImage, function( data ) {
		addImageArrayToBoard(data)
	});
}

function getSelectedImagesHTML() {
	return $('.image-container.selected');
}

function getImageFromImageContainer(imageContainer) {
	return $(imageContainer).find('img')[0];
}

function bindEvents() {
	$("#secondary-image-board").mCustomScrollbar({
		axis:'x',
		autoHideScrollbar: true
	});


	$('#secondary-image-board-toggle').click(function(){
		$(this).hide();
		$('#secondary-image-board, #secondary-image-board-title').show();
	});

	$('.secondary-image-board-exit').click(function(){
		$('#secondary-image-board-toggle').show();
		$('#secondary-image-board, #secondary-image-board-title').hide();
	});

	$('#image-board').on('click', '.image-container', function(){
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


	var $container = $('#image-board');
	// init
	$container.isotope({
	// options
		itemSelector: '.image-element',
		masonry: {
			columnWidth: 5
		}
	});


	$('#refresh').click(function() {
		location.reload();
	});

}

function initApp() {
	bindEvents();
	loadImages();
}

$(function(){

	initApp();

});
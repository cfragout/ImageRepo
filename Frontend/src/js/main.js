var baseURL = 'http://localhost:53079';
var apiUrl = baseURL + '/Api/';
var postImage = apiUrl + 'Imagen/PostImagen';
var getImage = apiUrl + 'Imagen/';

function createImageElement(path, name) {
	var alt = name || "";
	var imageElement = $('<div class="image-container image-element shadow" style="opacity:0"></div>');
	imageElement.append('<img class="b-lazy" data-src="'+ path +'" alt="' + alt.toLowerCase() +'">');

	return imageElement;
};

function addImageToBoard(image) {
	var imageContainer = createImageElement(image.path, image.name);
	var image = $(imageContainer).find('img');
	$(image).attr('src', $(image).attr('data-src'));
	var $container = $('#image-board');
	$container.isotope('insert', imageContainer)
}

function initImageBoard(imagesObjArray) {
	var $container = $('#image-board');
	var imageArray = [];

	$.each(imagesObjArray, function(index, image){
		imageArray.push(createImageElement(image.path, image.name)[0]);
	});

	$('#image-board').append(imageArray);

	var bLazy = new Blazy({
		container: '#image-board',
		success: function(ele){
			$(ele).parent().css('opacity', 1);
			$container.isotope('insert', $(ele).parent());
        },
		error: function(ele, msg){
			ele.src = "../assets/images/cannotLoadImg.png";
			$container.isotope('insert', $(ele).parent());
		}
	});
}

function loadImages() {
	$.get(getImage, function( data ) {
		console.log("data", data);
		initImageBoard(data)
	});
}

function getSelectedImagesHTML() {
	return $('.image-container.selected');
}

function getImageFromImageContainer(imageContainer) {
	return $(imageContainer).find('img')[0];
}

function findInImageBoard(query) {
	var $container = $('#image-board');
	var filterFunction = function() {
			var imageContainer = $(this).children('[alt*="'+ query.toLowerCase() +'"]')[0];
			return imageContainer != null;
		};

	if (query == '') {
		filterFunction = function() { return true; }
	}

	$container.isotope({
		filter: filterFunction
	});
}

function initElements() {
	// Secondary image board
	if (localStorage.hideSecondaryBoard == 'true') {
		toggleSecondaryBoard(false);
	} else {
		toggleSecondaryBoard(true);
	}
}

function bindEvents() {
	$('#quick-actions-more').click(function(){
		var actions = $('#quick-actions-more-container');

		if ($(actions).is(':visible')) {
			$('#quick-actions-more-container').slideUp();
			$('#quick-actions-more').find('span').attr('class', 'icon-arrow-down-5');
		} else {
			$('#quick-actions-more-container').slideDown();
			$('#quick-actions-more').find('span').attr('class', 'icon-arrow-up-5');
		}

	});

	$('#searchButton').click(function(){
		findInImageBoard($('#searchField').val());
		return false;
	});


	$("#secondary-image-board").mCustomScrollbar({
		axis:'x',
		autoHideScrollbar: true
	});


	$('#secondary-image-board-toggle').click(function(){
		toggleSecondaryBoard(true);
	});

	$('.secondary-image-board-exit').click(function(){
		toggleSecondaryBoard(false);
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
		onLayout: function() {
			$(window).trigger("scroll");
		},
		itemSelector: '.image-element',
		masonry: {
			columnWidth: 5
		}
	});





	$('#refresh').click(function() {
		location.reload();
	});

}

function toggleSecondaryBoard(shouldShow) {
	if (shouldShow) {
		$('#secondary-image-board-toggle').hide();
		$('#secondary-image-board, #secondary-image-board-title').show();
		localStorage.hideSecondaryBoard = false;
	} else {
		$('#secondary-image-board-toggle').show();
		$('#secondary-image-board, #secondary-image-board-title').hide();
		localStorage.hideSecondaryBoard = true;
	}
}

function initApp() {
	if (localStorage.hideSecondaryBoard == null) {
		localStorage.hideSecondaryBoard = false;
	}

	bindEvents();
	initElements();
	loadImages();

}

$(function(){

	initApp();

});
var baseUrl = 'http://localhost:55069/';
var apiUrl = baseUrl + 'Api/';
var postImageUrl = apiUrl + 'Imagenes/PostImage';
var uploadImageUrl = apiUrl + 'Imagenes/UploadImage';
var getImageUrl = apiUrl + 'Imagenes/GetImages';

function createImageElement(path, name) {
	console.log("baseUrl + path", baseUrl + path);
	var alt = name || "";
	var imageElement = $('<div class="image-container image-element shadow" style="opacity:0"></div>');
	imageElement.append('<img class="b-lazy" data-src="'+ path +'" alt="' + alt.toLowerCase() +'">');

	return imageElement;
};

function imageHasLoaded(image) {
	return !$(image).hasClass('img-load-error');
}

function addImageToBoard(image) {
	var imageContainer = createImageElement(image.Path, image.Name);
	var image = $(imageContainer).find('img');
	$(image).attr('src', $(image).attr('data-src'));
	var $container = $('#image-board');
	$container.isotope('insert', imageContainer)
}

function initImageBoard(imagesObjArray) {
	var $container = $('#image-board');
	var imageArray = [];
	$.each(imagesObjArray, function(index, image){
		imageArray.push(createImageElement(image.Path, image.Name)[0]);
	});

	$('#image-board').append(imageArray);

	var bLazy = new Blazy({
		container: '#image-board',
		success: function(ele) {
			console.log("success");
			if (isGifImage(ele) && (!localStorage.autoPlayGifs)) {
				freezeGif(ele);
			}
			$(ele).parents('.image-element').css('opacity', 1);
			$container.isotope('insert', $(ele).parents('.image-element'));
        },
		error: function(ele, msg) {
			console.log("error");
console.log("ele",ele, msg);
			// $(ele).attr('data-error-url', ele.src);
			ele.src = "../assets/images/cannotLoadImg.png";
			$(ele).addClass('img-load-error');
			$container.isotope('insert', $(ele).parents('.image-element'));
		}
	});
}

function loadImages() {
	$.get(getImageUrl, function( data ) {
		console.log("data", data);
		initImageBoard(data)
	});
}

function getSelectedImagesHTMLContainer() {
	return $('.image-container.selected');
}

function getFirstSelectedImageHTMLContainer(){
	return $(getSelectedImagesHTMLContainer()[0]).find('img');
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
	$('#closeOptionsScreenContainer').click(function(){
		$('#mainNavbar, #secondary-image-board-row, #mainGrid').show().addClass('animated fadeInLeftBig');
		$('#optionsScreen').addClass('animated rotateOutDownRight');

		$('#optionsScreen').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function(){
			$('#optionsScreen').removeClass('animated rotateOutDownRight').hide();
		});
		$('#mainNavbar, #secondary-image-board-row, #mainGrid').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function(){
			$('#mainNavbar, #secondary-image-board-row, #mainGrid').removeClass('animated fadeInLeftBig');
		});
	});

	$('#showOptions').click(function(){
		$('#mainNavbar, #secondary-image-board-row, #mainGrid').addClass('animated fadeOutLeftBig');
		$('#optionsScreen').show().addClass('animated rotateInUpRight');

		$('#mainNavbar, #secondary-image-board-row, #mainGrid').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function(){
			$('#mainNavbar, #secondary-image-board-row, #mainGrid').removeClass('animated fadeOutLeftBig').hide();
		});
		$('#optionsScreen').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function(){
			$('#optionsScreen').removeClass('animated fadeOutLeftBig');
		});
	});




	$('#image-board').on('click', '.play-gif-icon', function(){
		var image = $(this).siblings('img');

		var staticImage = $(image).attr('src');
		$(image).attr('src', $(image).attr('data-gif'));
		$(image).attr('data-static-img', staticImage);
		$(image).removeAttr('data-gif');

		$(this).addClass('stop-gif-icon icon-stop-2').removeClass('play-gif-icon icon-play-alt');

		// Prevent .selected
		return false;
	});

	$('#image-board').on('click', '.stop-gif-icon', function(){
		var image = $(this).siblings('img');

		$(image).attr('data-gif', $(image).attr('src'));
		$(image).attr('src', $(image).attr('data-static-img'));
		$(image).removeAttr('data-static-img')

		$(this).addClass('play-gif-icon icon-play-alt').removeClass('stop-gif-icon icon-stop-2');

		// Prevent .selected
		return false;
	});



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


		var selectedCount = getSelectedImagesHTMLContainer().length;
		if (selectedCount == 1) {
			var selectedImage = getFirstSelectedImageHTMLContainer();
			if (!$(selectedImage).hasClass('img-load-error')) {
				$('.actions span').removeClass('fg-gray no-hover');
				$('.actions').removeClass('no-hover');
			}
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

		var selectedImage = getFirstSelectedImageHTMLContainer();
		if (!imageHasLoaded(selectedImage)) {
			link.removeAttr('href');
			return;
		}

		var selectedImages = getSelectedImagesHTMLContainer();

		if ((selectedImages.length == 0) ||(selectedImages.length > 1)) {
			link.removeAttr('href');
			return;
		}

		var imageHTML = getImageFromImageContainer(selectedImages[0]);
		var serverUrl = $(imageHTML).attr('data-gif') || $(imageHTML).attr('src');
		link.attr('href', serverUrl);
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


/**/
function isGifImage(i) {
    return /^(?!data:).*\.gif/i.test(i.src);
}

function freezeGif(image) {
	var c = document.createElement('canvas');
	var w = c.width = image.width;
	var h = c.height = image.height;
	c.getContext('2d').drawImage(image, 0, 0, w, h);
	try {
		$(image).attr('data-gif', image.src);
		image.src = c.toDataURL("image/gif"); // if possible, retain all css aspects

		// Create play overlay
		$(image).wrap('<div class="gif-wrapper">');
		var wrapper = $(image).parent();
		$(wrapper).append('<span class="not-selectable play-gif-icon icon-play-alt">');
	} catch(e) { // cross-domain -- mimic original with all its tag attributes
		for (var j = 0, a; a = image.attributes[j]; j++) {
			c.setAttribute(a.name, a.value);
		}
		image.parentNode.replaceChild(c, image);
	}
}
/**/






$(function(){

	initApp();

});
var baseUrl = 'http://localhost:55069/';
var imagesApiUrl = baseUrl + 'api/Imagenes/';
var tagsApiUrl = baseUrl + 'api/Tags/';
var postImageUrl = imagesApiUrl + 'PostImage';
var uploadImageUrl = imagesApiUrl + 'UploadImage';
var getImageUrl = imagesApiUrl + 'GetImages';
var markFavouriteUrl = imagesApiUrl + 'MarkImagesAsFavourite'
var removeTagUrl = imagesApiUrl + 'RemoveTag';
var putImageUrl = imagesApiUrl + 'PutImagen/';
var putTagUrl = tagsApiUrl + 'PutTag/';

function storeImages(images) {
	var imgs = [];
	$.each(images, function(index, img){
		imgs[img.Id] = img;
	});

	localStorage.images = JSON.stringify(imgs);
}

function updateImage(image) {
	var images = JSON.parse(localStorage.images);

	images[image.Id] = image;
	localStorage.images = JSON.stringify(images);
}

function getImageById(id) {
	var images = JSON.parse(localStorage.images);

	if (images[id] != null) {
		return images[id];
	}

	return "";
}

function getImageIdbyImageHTML(imageHTML) {
	return imageHTML.id.split('-')[1];
}

function getImageObjectByImageHTML(imageHTML) {
	return getImageById(getImageIdbyImageHTML(imageHTML));
}

function createImageElement(image) {
	var alt = image.Name || "";
	var id = parseInt(image.Id, 10)
	var imageElement = $('<div id="container-' + id +'" class="image-container image-element shadow" style="opacity:0"></div>');

	imageElement.append('<img id="img-' + id + '" class="b-lazy" data-src="'+ image.Path +'" alt="' + alt +'">');

	return imageElement;
};

function imageHasLoaded(image) {
	return !$(image).hasClass('img-load-error');
}

function addImageToBoard(image) {
	var imageContainer = createImageElement(image);
	var imageHTML = $(imageContainer).find('img')[0];
	$(imageHTML).attr('src', $(imageHTML).attr('data-src'));

	if (isGifImage(imageHTML) && (localStorage.autoPlayGifs == "false")) {
		freezeGif(imageHTML);
	}

	var $container = $('#image-board');
	$container.isotope({
		filter: function() { return true; }
	});
	$container.isotope('insert', imageContainer);

	updateImage(image);
}

function addImageToSecondaryBoard(image) {
	var imageContainerHTML = $('<div id="thumbnail-container-'+ image.Id +'" class="secondary-image-element"></div>')[0];
	var imageHTML = $('<img id="thumbnail-' + image.Id +'" src="'+ image.Path +'" class="shadow">')[0];

	if (isGifImage(imageHTML) && (localStorage.autoPlayGifs == "false")) {
		freezeGif(imageHTML);
		$(imageContainerHTML).append($(imageHTML).parent());
	} else {
		$(imageContainerHTML).append($(imageHTML));
	}

	// Append to scroll plugin container
	$('#mCSB_1_container').append(imageContainerHTML);
}

function removeImageFromSecondaryBoard(imageObjId) {
	$("#secondary-image-board").find('#thumbnail-container-' + imageObjId).remove();
}

function initImageBoard(imagesObjArray) {
	var $container = $('#image-board');
	var imageArray = [];
	$.each(imagesObjArray, function(index, image){
		if (image != null) {
			var imageContainerHTML = createImageElement(image)[0];
			imageArray.push(imageContainerHTML);
		}
	});

	$('#image-board').append(imageArray);

	var bLazy = new Blazy({
		container: '#image-board',
		success: function(ele) {
			if (isGifImage(ele) && (localStorage.autoPlayGifs == "false")) {
				freezeGif(ele);
			}

			// Add image to primary board
			$(ele).parents('.image-container').css('opacity', 1);
			$container.isotope('insert', $(ele).parents('.image-container'));

			// If favourite add image to secondary board
			var addedImg = getImageObjectByImageHTML(ele);
			if (addedImg.IsFavourite) {
				addImageToSecondaryBoard(addedImg);
			}
        },
		error: function(ele, msg) {
			console.log("error", ele);

			ele.src = "../assets/images/cannotLoadImg.png";
			$(ele).addClass('img-load-error');

			// Remove class so that it has no click handler
			$(ele).parents('.image-container').removeClass('image-element');

			$container.isotope('insert', $(ele).parents('.image-container'));
		}
	});

	$('#image-board-loader-container').hide();
}

function resetBoard() {
	$('#image-board-loader-container').show();

	var $container = $('#image-board');
	var items = $container.isotope('getItemElements');

    $container.isotope('remove', items);
    initIsotope();
	loadImages();
}

function loadImages() {
	$.get(getImageUrl, function( data ) {
		console.log("data", data);
		storeImages(data);
		initImageBoard(data);
	});
}

function getSelectedImagesHTMLContainer() {
	return $('.image-container.selected');
}

function getFirstSelectedImageHTMLContainer() {
	// Returns the html container of the first selected image
	return $(getSelectedImagesHTMLContainer()[0]).find('img');
}

function getFirstSelectedImage() {
	// Returns the image object that is represented by the first selected img html container
	var selectedImage = getFirstSelectedImageHTMLContainer();
	return getImageObjectByImageHTML(selectedImage[0]);
}

function getImageHTMLFromImageContainer(imageContainer) {
	return $(imageContainer).find('img')[0];
}

function findTagsInArray(tagArray, sbr, strict) {
	// Returns tags with name sbr (if strict) or with sbr substring in the tag name.
	var results = [];

	$.each(tagArray, function(index, tag) {
		var name = tag.Name || '';

		if (strict) {
			if (name.toLowerCase() == sbr) {
				results.push(tag);
			}
		} else {
			if (name.toLowerCase().indexOf(sbr.toLowerCase()) > -1) {
				results.push(tag);
			}
		}
	});

	return results;
}

function filterByTagName(imageContainer, query, strict) {
	// isotope Filter
	var imageObj = getImageObjectByImageHTML(getImageHTMLFromImageContainer(imageContainer));
	var q = query || '';
	var tags = findTagsInArray(imageObj.Tags, q, strict);
	return tags.length > 0;
}

function filterByImageName(imageContainer, query, strict) {
	// isotope Filter
	var q = query || '';
	var imageAlt =  $(imageContainer).children().attr('alt');

	if (!strict) {
		q = q.toLowerCase();
		imageAlt = imageAlt.toLowerCase();

		return imageAlt.indexOf(q) > -1
	}

	return q == imageAlt;
}

function findByTagNameInImageBoard(tagName, strict) {
	var $container = $('#image-board');
	var filterFunction = function() {
			return filterByTagName(this, tagName, strict);
		};

	if (tagName == '') {
		filterFunction = function() { return true; }
	}

	$container.isotope({
		filter: filterFunction
	});
}

function findInImageBoard(query, strict) {
	var $container = $('#image-board');
	var filterFunction = function() {
			return filterByImageName(this, query, strict) || filterByTagName(this, query, strict);
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

	initIsotope();
}

function setFavouriteSelectedIcon() {
	$('#markFavourite').find('span')
						.removeClass('icon-heart-2')
						.addClass('icon-heart');
}

function removeFavouriteSelectedIcon() {
	$('#markFavourite').find('span')
						.removeClass('icon-heart')
						.addClass('icon-heart-2');
}

function createTagListElement(tag, imageId) {
	return '<li class="sidebar-tag-line"><a href="#">'+ tag.Name +'<span class="hidden remove-tag-icon icon-cancel-2 pull-right" data-tag-id="'+ tag.Id +'" data-image-id="'+ imageId +'"></span></a></li>';
}

function updateSidebarTagList(image) {
	$('#image-tag-list ul').empty();

	if (image == null) {
		return;
	}

	$.each(image.Tags, function(index, tag){
		$('#image-tag-list ul').append(createTagListElement(tag, image.Id));
	});
}

function initIsotope(){
	var $container = $('#image-board');
	$container.isotope({
		onLayout: function() {
			$(window).trigger("scroll");
		},
		itemSelector: '.image-container',
		masonry: {
			columnWidth: 5
		}
	});
}

function isStrictSearch(q) {
	// Parses string. If starts and ends with '' or "" then it is a strict search.

	return ((q.match('^"') != null) && (q.match('"$') != null)) ||
			((q.match("^'") != null) && (q.match("'$") != null))
}

function bindEvents() {
	// Sidebar: find by tag
	$('#image-tag-list').on('click', '.sidebar-tag-line', function(){
		var tagName = $(($(this).find('a')[0])).text();
		// findByTagNameInImageBoard(tagName, true);

		$('#searchField').val('Tag: "' + tagName + '"');
		$('#searchButton').trigger('click');
	});


	// Quick actions: Remove Tag
	$('#image-tag-list').on('click', '.remove-tag-icon', function(){
		var imageId = $(this).attr('data-image-id');
		var tagId = $(this).attr('data-tag-id');
		var self = this;

		$.ajax({
			url: removeTagUrl,
			data: { tagId: tagId, imageId: imageId},
			type: 'POST',
			success: function(succeded) {
				if (succeded) {
					$(self).closest('li.sidebar-tag-line').remove();

					var updatedImage = getImageById(imageId);
					for (var i = 0; i < updatedImage.Tags.length; i++) {
						if (updatedImage.Tags[i].Id == tagId) {
							updatedImage.Tags.splice(i, 1);
							updateImage(updatedImage);
							break;
						}
					}

				}
			},
			error: function(data) {
				$.Notify({
					style: {background: '##9a1616', color: 'white'},
					caption: 'Ups...',
					content: "El tag no se pudo borrar. Intentalo nuevamente.",
					timeout: 5000
				});
			}
		});

		return false;
	});

	// Quick actions: delete image
	$('#deleteImage').click(function() {
		if ($('#deleteImage span').hasClass('fg-gray')) {
			return;
		}

		var image = getFirstSelectedImage();
		image.IsDeleted = true;

		$.ajax({
			url: putImageUrl + image.Id,
			contentType: "application/json; charset=utf-8",
			data: JSON.stringify(image),
			type: 'PUT',
			success: function(data) {
				updateImage(image);
				$container = $('#image-board');
				$container.isotope('remove', $('.image-container.selected')[0]);
			},
			error: function() {
				$.Notify({
					style: {background: '##9a1616', color: 'white'},
					caption: 'Ups...',
					content: "No se pudo eliminar la imagen",
					timeout: 5000
				});
			}
		});
	});

	// Quick actions: favourite
	$('#markFavourite').click(function(){
		if ($('#markFavourite span').hasClass('fg-gray')) {
			return;
		}

		var selectedIds = [];

		getSelectedImagesHTMLContainer().each(function(index, container){
			selectedIds.push(container.id.split('-')[1]);
		});

		jQuery.ajaxSettings.traditional = true;

		$.ajax({
			type: "POST",
			url: markFavouriteUrl,
			data: { imagesIds : selectedIds },
			success: function(data) {
				$.each(data, function(index, id){
					var image = getImageById(id);

					if (image.IsFavourite) {
						console.log('remove:', image)
						removeImageFromSecondaryBoard(image.Id);
					} else {
						console.log('add:', image)
						addImageToSecondaryBoard(image);
					}

					image.IsFavourite = !image.IsFavourite;
					updateImage(image);

				});

				if (getFirstSelectedImage().IsFavourite) {
					setFavouriteSelectedIcon();
				}
			},
			error: function() {
			}
		});

	});


	// Primary image board: gif play
	$('#image-board').on('click', '.play-gif-icon', function(){
		var image = $(this).siblings('img');

		var staticImage = $(image).attr('src');
		$(image).attr('src', $(image).attr('data-gif'));
		$(image).attr('data-static-img', staticImage);
		$(image).removeAttr('data-gif');

		$(this).addClass('stop-gif-icon icon-pause').removeClass('play-gif-icon icon-play-alt');

		// Prevent .selected
		return false;
	});

	// Primary image board: gif stop
	$('#image-board').on('click', '.stop-gif-icon', function(){
		var image = $(this).siblings('img');

		$(image).attr('data-gif', $(image).attr('src'));
		$(image).attr('src', $(image).attr('data-static-img'));
		$(image).removeAttr('data-static-img')

		$(this).addClass('play-gif-icon icon-play-alt').removeClass('stop-gif-icon icon-pause');

		// Prevent .selected
		return false;
	});


	// Quick actions: more actions
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

	// Navbar: search
	$('#searchButton').click(function() {
		var query = $('#searchField').val();

		// Remove .selected
		$('.image-container.selected').trigger('click');

		if (query.toLowerCase().indexOf('tag:') > -1) {
			// Searching criteria: tags...
			query = query.toLowerCase().split('tag:')[1];
			query = query.trim();

			if (isStrictSearch(query)) {
				query = query.substring(1, query.length-1);
				findByTagNameInImageBoard(query, true); // If search query is enclosed in '' or "", perform strict search
			} else {
				findByTagNameInImageBoard(query.trim());
			}

		} else {

			// No search criteria
			if (isStrictSearch(query)) {
				query = query.substring(1, query.length-1);
				findInImageBoard(query, true);
			} else {
				findInImageBoard(query.trim());
			}

		}

		return false;
	});

	// scrolls
	$('#secondary-image-board').mCustomScrollbar({
		axis:'x',
		autoHideScrollbar: true
	});
	$('#image-tag-list').mCustomScrollbar({
		autoHideScrollbar: true,
        setHeight: 220
	});

	// Sidebar: remove tag icon
	$('#image-tag-list').on('mouseenter', '.sidebar-tag-line', function(){
		$(this).find('.remove-tag-icon').show();
	});
	$('#image-tag-list').on('mouseleave', '.sidebar-tag-line', function(){
		$(this).find('.remove-tag-icon').hide();
	});


	// Secondary board toggle
	$('#secondary-image-board-toggle').click(function(){
		toggleSecondaryBoard(true);
	});

	$('.secondary-image-board-exit').click(function(){
		toggleSecondaryBoard(false);
	});

	// Primary board: image click
	$('#image-board').on('click', '.image-element', function(){
		$(this).toggleClass('selected');

		var selectedCount = getSelectedImagesHTMLContainer().length;
		var selectedImage = getFirstSelectedImageHTMLContainer();

		if (selectedCount == 0) {
			removeFavouriteSelectedIcon();
			updateSidebarTagList();
			$('#copy-image-url').val('');
			$('#sidebar-image-name').text('-');
			$('.actions span').addClass('fg-gray no-hover');
			$('.actions').addClass('no-hover');
		} else if (selectedCount == 1) {

			$('.actions span').removeClass('fg-gray no-hover');
			$('.actions').removeClass('no-hover');

			var image = getFirstSelectedImage();
			if (image.IsFavourite) {
				setFavouriteSelectedIcon();
			} else {
				removeFavouriteSelectedIcon();
			}

			updateSidebarTagList(image);
			$('#copy-image-url').val(image.Path).select();
			$('#sidebar-image-name').text(image.Name);



		} else if (selectedCount > 1) {
			$('.actions .icon-link').addClass('fg-gray no-hover');
			$('.actions span.icon-link').closest('.actions').addClass('no-hover');
		} else {
			$('.actions span').addClass('fg-gray no-hover');
			$('.actions').addClass('no-hover');
		}

	});


	// Quick actions: link
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

		var imageHTML = getImageHTMLFromImageContainer(selectedImages[0]);
		var serverUrl = $(imageHTML).attr('data-gif') || $(imageHTML).attr('src');
		link.attr('href', serverUrl);
	});



	$('#refresh').click(function() {
		location.reload();
	});

}

function toggleSecondaryBoard(shouldShow) {
	if (shouldShow) {
		$('#secondary-cont').css('visibility','visible');
		$('#secondary-image-board-toggle').addClass('animated fadeOutDown');
		$('#secondary-cont').show().addClass('animated fadeInUp');

		$('#secondary-image-board-toggle').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function(){
			$('#secondary-image-board-toggle').removeClass('animated fadeOutDown').hide();
		});
		$('#secondary-cont').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function(){
			$('#secondary-cont').removeClass('animated fadeOutDown');
		});

		$("#secondary-image-board").mCustomScrollbar("scrollTo", 0);
		localStorage.hideSecondaryBoard = false;
	} else {
		$('#secondary-image-board-toggle').show().addClass('animated fadeInUp');;
		$('#secondary-cont').addClass('animated fadeOutDown');
		localStorage.hideSecondaryBoard = true;



		$('#secondary-cont').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function(){
			$('#secondary-cont').removeClass('animated fadeOutDown').hide();
		});
		$('#secondary-image-board-toggle').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function(){
			$('#secondary-image-board-toggle').removeClass('animated rotateInUpRight');
		});
	}
}

function initApp() {
	$('#image-board-loader-container').show();

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
	var canvasImg = new Image();
	var imageLoaded = (image.width != 0) && (image.height != 0);
	var noPreviewSrc = '../assets/images/previewNotAvailable.png';
	c.width = 180;
	c.height = 240;


	if (imageLoaded) {
		c.width = image.width;
		c.height = image.height;
	}

	var w = c.width;
	var h = c.height;

	c.getContext('2d').drawImage(image, 0, 0, w, h);
	try {

		$(image).attr('data-gif', image.src);
		if (imageLoaded) {
			image.src = c.toDataURL("image/gif"); // if possible, retain all css aspects
		} else {
			 image.src = '../assets/images/previewNotAvailable.png';
		}

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
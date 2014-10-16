var shouldUpdate = false;

function getDropdownSelectedItems(dropdownId) {
	console.log("dropdownId", dropdownId);
	var selectedItemsIds = [];
	$('#'+ dropdownId +' :selected').each(function(i, selected) {
		selectedItemsIds[i] = selected;
	});

	$('#' + dropdownId).trigger('click');

	return selectedItemsIds;
}

function addOptionToDropdown(dropdownId, optionValue, optionText) {
	var o = new Option(optionText, optionValue);
	$('#' + dropdownId).append(o);
}

function sendPUTTag(option, hide, selectID) {
	console.log("$(option).val()", option)
	$.ajax({
		url: putTagUrl + $(option).val(),
		contentType: "application/json; charset=utf-8",
		data: JSON.stringify({
			Id: $(option).val(),
			Name: $(option).text(),
			IsHidden: hide
		}),
		type: 'PUT',
		success: function(data) {
			addOptionToDropdown(selectID, $(option).val(), $(option).text());
			$(option).remove();
			shouldUpdate = true;
		},
		error: function() {
			$.Notify({
				style: {background: '##9a1616', color: 'white'},
				caption: 'Ups...',
				content: "No se pudieron guardar los cambios",
				timeout: 5000
			});
		}
	});
}

// Options section: backup
$('#backupButton').click(function(){
	$.ajax({
		url: backupUrl,
		type: 'GET',
		success: function(data) {
			console.log(data);
			$('#REMOVEME').append('<a id="backupDownloadLink" href="'+ data +'">Descargar imagenes<a/>');
		},
		error: function() {
			$.Notify({
				style: {background: '##9a1616', color: 'white'},
				caption: 'Ups...',
				content: "Hubo un problema al completar el backup. Intenta mas tarde",
				timeout: 5000
			});
		}
	});
});


// Options section: close
$('#closeOptionsScreenContainer').click(function(){
	$('#mainNavbar, #secondary-image-board-row, #mainGrid').show().addClass('animated fadeInLeftBig');
	$('#optionsScreen').addClass('animated rotateOutDownRight');

	$('#optionsScreen').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function(){
		$('#optionsScreen').removeClass('animated rotateOutDownRight').hide();
	});
	$('#mainNavbar, #secondary-image-board-row, #mainGrid').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function(){
		$('#mainNavbar, #secondary-image-board-row, #mainGrid').removeClass('animated fadeInLeftBig');
	});

	if (shouldUpdate) {
		resetBoard();
	}

	$('#backupDownloadLink').remove();
});

// Options section: open
$('#showOptions').click(function(){
	shouldUpdate = false;

	$.ajax({
		url: getTagsUrl,
		success: function(data) {
			$('#hidden-tags-list option, #visible-tags-list option').remove();

			$.each(data, function(index, tag) {
				if (tag.IsHidden) {
					addOptionToDropdown('hidden-tags-list', tag.Id, tag.Name);
				} else {
					addOptionToDropdown('visible-tags-list', tag.Id, tag.Name);
				}
			});
		},
		error: function() {
			console.log('Error al traer los tags');
		}
	});

	$('#mainNavbar, #secondary-image-board-row, #mainGrid').addClass('animated fadeOutLeftBig');
	$('#optionsScreen').show().addClass('animated rotateInUpRight');

	$('#mainNavbar, #secondary-image-board-row, #mainGrid').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function(){
		$('#mainNavbar, #secondary-image-board-row, #mainGrid').removeClass('animated fadeOutLeftBig').hide();
	});
	$('#optionsScreen').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function(){
		$('#optionsScreen').removeClass('animated fadeOutLeftBig');
	});
});

// Options: hide tags
$('#visible-tags-list').click(function(){
	if ($('#visible-tags-list :selected').length > 0) {
		$('#move-tag-left').removeClass('arrow-disabled');
		$('#move-tag-left span').removeClass('fg-gray');
	} else {
		$('#move-tag-left').addClass('arrow-disabled');
		$('#move-tag-left span').addClass('fg-gray');
	}
});

$('#hidden-tags-list').click(function(){
	if ($('#hidden-tags-list :selected').length > 0) {
		$('#move-tag-right').removeClass('arrow-disabled');
		$('#move-tag-right span').removeClass('fg-gray');
	} else {
		$('#move-tag-right').addClass('arrow-disabled');
		$('#move-tag-right span').addClass('fg-gray');
	}

});

$('#move-tag-right').click(function(){
	if ($(this).hasClass('arrow-disabled')) {
		return;
	}

	var selected = getDropdownSelectedItems("hidden-tags-list");

	$.each(selected, function(index, option) {
		sendPUTTag(option, false, 'visible-tags-list');
	});
});

$('#move-tag-left').click(function(){
	if ($(this).hasClass('arrow-disabled')) {
		return;
	}

	var selected = getDropdownSelectedItems("visible-tags-list");

	$.each(selected, function(index, option) {
		sendPUTTag(option, true, 'hidden-tags-list');
	});

});

$('#optionsList li').click(function(){
	$('#optionsList li').removeClass('selected');
	$(this).addClass('selected');

	$('.option-content:visible').hide();
	var selectedOption = $(this).attr('data-option-id');
	$(selectedOption).show().addClass('animated fadeInRight');
});
// End

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
	filename = name;
	readURL(this, $('#profilePicture'));
});
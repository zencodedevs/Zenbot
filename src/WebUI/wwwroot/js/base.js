function initializePasswordInput(togglePasswordButton, passwordInput, isShowPassword) {
    let hideElementCssClass = 'd-none';
    let togglePasswordButtonVisibleIcon = $(togglePasswordButton).find('*[data-is-toggle-password-visible-icon="true"]');
    let togglePasswordButtonHiddenIcon = $(togglePasswordButton).find('*[data-is-toggle-password-hidden-icon="true"]');

    $(togglePasswordButtonVisibleIcon).addClass(hideElementCssClass);
    $(togglePasswordButtonHiddenIcon).addClass(hideElementCssClass);

    if (isShowPassword) {
        $(passwordInput).attr('type', 'text');
        $(togglePasswordButtonVisibleIcon).removeClass(hideElementCssClass);
    } else {
        $(passwordInput).attr('type', 'password');
        $(togglePasswordButtonHiddenIcon).removeClass(hideElementCssClass);
    }
}

$(document).ready(function () {
    $('button[data-is-toggle-password-button="true"]').each(function () {
        let passwordInput = $(this)
            .parents('div.input-group')
            .find('input[data-is-password-input="true"]');

        let isShowPassword = $(passwordInput).attr('type') == 'text';

        initializePasswordInput(this, passwordInput, isShowPassword);

        $(this).on('click', function () {
            isShowPassword = !isShowPassword;
            initializePasswordInput(this, passwordInput, isShowPassword);
        });
    });
});

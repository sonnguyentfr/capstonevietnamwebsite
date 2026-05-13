CKEDITOR.plugins.add('dangkydo', {
    icons: 'dangkydo',
    init: function (editor) {
        editor.addCommand('dangkydo', new CKEDITOR.dialogCommand('dangkydoDialog'));
        editor.ui.addButton('dangkydo', {
            label: 'Chèn nút đăng ký',
            command: 'dangkydo',
            toolbar: 'insert'
        });
        CKEDITOR.dialog.add('dangkydoDialog', function (editor) {
            return {

                // Basic properties of the dialog window: title, minimum size.
                title: 'Thêm nút đăng ký',
                minWidth: 600,
                minHeight: 150,

                // Dialog window content definition.
                contents: [
                    {
                        // Definition of the Basic Settings dialog tab (page).
                        id: 'tab-basic',
                        label: 'Basic Settings',

                        // The tab content.
                        elements: [
                            {
                                // Text input field for the abbreviation text.
                                type: 'text',
                                id: 'abbr',
                                label: 'Link đăng ký: ',
                                validate: CKEDITOR.dialog.validate.notEmpty("Link đăng ký không để trống")
                            }
                        ]
                    }
                ],

                // This method is invoked once a user clicks the OK button, confirming the dialog.
                onOk: function () {
                    var dialog = this;
                    var content = '';
                    // Create a new <abbr> element.
                    content = "<p style='text-align:center'><a target=_blank href='" + this.getValueOf('tab-basic', 'abbr') + "'><img src='/images/Dang%20ky-01.jpg' alt='Đăng ký' /></a></p>";
                    // Now get yet another field value from the Advanced Settings tab.
                    editor.insertHtml(content);
                }
            };
        });

    }
});
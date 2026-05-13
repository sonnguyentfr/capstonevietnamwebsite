CKEDITOR.plugins.add('insertpdf', {
    icons: 'insertpdf',
    init: function (editor) {
        editor.addCommand('insertpdf', new CKEDITOR.dialogCommand('insertpdfDialog'));
        editor.ui.addButton('insertpdf', {
            label: 'Chèn file PDF',
            command: 'insertpdf',
            toolbar: 'insert'
        });
        CKEDITOR.dialog.add('insertpdfDialog', function (editor) {
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
                                label: 'Link file pdf: ',
                                validate: CKEDITOR.dialog.validate.notEmpty("Link  không để trống")
                            }
                        ]
                    }
                ],

                // This method is invoked once a user clicks the OK button, confirming the dialog.
                onOk: function () {
                    var dialog = this;
                    var content = '';
                    // Create a new <abbr> element.
                    content = "<iframe style='height:800px; width:100%' width='100%' height='800px'src='" + this.getValueOf('tab-basic', 'abbr') + "'></iframe>";
                    // Now get yet another field value from the Advanced Settings tab.
                    editor.insertHtml(content);
                }
            };
        });

    }
});
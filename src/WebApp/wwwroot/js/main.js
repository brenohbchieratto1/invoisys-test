$.ajaxPrefilter(function(options) {
    const baseUrl = 'http://localhost:5215';
    if (!options.url.startsWith('http')) {
        options.url = baseUrl + options.url;
    }
});
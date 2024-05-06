/** @type {import('next').NextConfig} */
const nextConfig = {
    images: {
        domains: [
            'i.ibb.co',
            'images.unsplash.com',
            'cdn.pixabay.com',
            'lh3.googleusercontent.com',
            'pinterest.com',
            'flickr.com',
            'static.pexels.com',
            'media.istockphoto.com',
            'files.wordpress.com',
            'img.youtube.com',
            'instagram.com',
            'cdn.shopify.com',
            'cdn.vox-cdn.com',
            'static.wikia.nocookie.net',
            'pbs.twimg.com',
            'upload.wikimedia.org',
            'm.media-amazon.com',
            'cdn.discordapp.com'
        ],
    },
    output: 'standalone'
};

export default nextConfig;
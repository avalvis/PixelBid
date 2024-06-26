'use client'

import React, { useState } from 'react'
import Image from 'next/image'

type Props = {
    imageUrl: string
}

export default function ItemImage({ imageUrl }: Props) {
    const [isLoading, setLoading] = useState(true);
    return (
        <Image
            src={imageUrl}
            alt='image'
            fill
            priority
            className={`
                object-cover
                group-hover:opacity-80
                duration-500
                ease-in-out
                hover:scale-105
                ${isLoading ? 'grayscale blur-2xl scale-120' : 'grayscale-0 blur-0 scale-100'}
            `}
            sizes='(max-width:768px) 100vw, (max-width: 1200px) 50vw, 25vw'
            onLoadingComplete={() => setLoading(false)}
        />
    )
}
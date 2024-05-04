import React from 'react'

type Props = {
    title: string
    subtitle?: string
    className?: string
}

export default function Heading({ title, subtitle, className }: Props) {
    return (
        <div className={className}>
            <div className='text-2xl font-bold text-center'>
                {title}
            </div>
            <div className='font-light text-neutral-500 mt-2 text-center'>
                {subtitle}
            </div>
        </div>
    )
}
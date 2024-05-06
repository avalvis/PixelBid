'use client'

import { Button, TextInput } from 'flowbite-react';
import React, { useEffect } from 'react'
import { FieldValues, useForm } from 'react-hook-form'
import Input from '../components/input';
import DateInput from '../components/DateInput';
import { usePathname, useRouter } from 'next/navigation';
import { toast } from 'react-hot-toast';
import { Auction } from '@/types';
import { createAuction, updateAuction } from '../actions/auctionActions';
import Link from 'next/link';


type Props = {
    auction?: Auction
}

export default function AuctionForm({ auction }: Props) {
    const router = useRouter();
    const pathname = usePathname();
    const { control, handleSubmit, setFocus, reset,
        formState: { isSubmitting, isValid } } = useForm({
            mode: 'onTouched'
        });

    useEffect(() => {
        if (auction) {
            const { platform, title, genre, playHours, year } = auction;
            reset({ platform, title, genre, playHours, year });
        }
        setFocus('platform');
    }, [setFocus, reset, auction])

    async function onSubmit(data: FieldValues) {
        try {
            let id = '';
            let res;
            if (pathname === '/auctions/create') {
                res = await createAuction(data);
                id = res.id;
            } else {
                if (auction) {
                    res = await updateAuction(data, auction.id);
                    id = auction.id;
                }
            }
            if (res.error) {
                throw res.error;
            }
            router.push(`/auctions/details/${id}`)
        } catch (error: any) {
            toast.error('Ooops! ' + error.status + ' ' + error.message)
        }
    }

    return (
        <form className='flex flex-col mt-3' onSubmit={handleSubmit(onSubmit)}>
            <Input label='Platform' name='platform' control={control}
                rules={{ required: 'Platform is required' }} />
            <Input label='Title' name='title' control={control}
                rules={{ required: 'Title is required' }} />
            <Input label='Genre' name='genre' control={control}
                rules={{ required: 'Genre is required' }} />

            <div className='grid grid-cols-2 gap-3'>
                <Input label='Year' name='year' control={control} type='number'
                    rules={{ required: 'Year is required' }} />
                <Input label='Hours Played' name='playHours' control={control} type='number'
                    rules={{ required: 'Hours played are required' }} />
            </div>

            {pathname === '/auctions/create' &&
                <>
                    <Input label='Image URL' name='imageUrl' control={control}
                        rules={{ required: 'Image URL is required' }} />

                    <div className='grid grid-cols-2 gap-3'>
                        <Input label='Reserve Price (Enter 0 if you do not want to set a reserve price)'
                            name='reservePrice' control={control} type='number'
                            rules={{ required: 'Reserve price is required' }} />
                        <DateInput
                            label='Auction end date/time'
                            name='auctionEnd'
                            control={control}
                            dateFormat='dd MMMM yyyy h:mm a'
                            showTimeSelect
                            rules={{ required: 'Auction end date is required' }} />
                    </div>
                </>}


            <div className='flex justify-between'>
                <Link href="/">
                    <Button outline color='gray'>Cancel</Button>
                </Link>
                <Button
                    isProcessing={isSubmitting}
                    disabled={!isValid}
                    type='submit'
                    outline color='success'>Submit</Button>
            </div>
        </form>
    )
}

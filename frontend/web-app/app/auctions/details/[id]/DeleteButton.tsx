'use client';

import { deleteAuction } from '@/app/actions/auctionActions';
import { Button } from 'flowbite-react';
import React, { useState } from 'react'
import { toast } from 'react-hot-toast';

type Props = {
    id: string
}

export default function DeleteButton({ id }: Props) {
    const [loading, setLoading] = useState(false);

    async function doDelete() {
        const confirmDelete = window.confirm('Are you sure you want to delete your auction?');

        if (!confirmDelete) {
            return;
        }

        setLoading(true);
        try {
            const res = await deleteAuction(id);
            if (res.error) throw res.error;
            window.location.href = '/';
        } catch (error) {
            const err = error as Error;
            toast.error('Ooops' + err.message);
        } finally {
            setLoading(false);
        }
    }

    return (
        <Button color='failure' isProcessing={loading} onClick={doDelete}>
            Delete
        </Button>
    )
}
import { useParamsStore } from '@/hooks/useParamsStore';
import { Button } from 'flowbite-react';
import React from 'react'
import { AiOutlineClockCircle, AiOutlineSortAscending } from 'react-icons/ai';
import { BsFillStopCircleFill, BsStopwatchFill } from 'react-icons/bs';
import { GiFinishLine, GiFlame } from 'react-icons/gi';

const pageSizeButtons = [4, 8, 12];

const orderButtons = [
    {
        label: 'AZ',
        icon: AiOutlineSortAscending,
        value: 'title'
    },
    {
        label: 'Ending',
        icon: AiOutlineClockCircle,
        value: 'endingSoon'
    },
    {
        label: 'Recently added',
        icon: BsFillStopCircleFill,
        value: 'new'
    },
]

const filterButtons = [
    {
        label: 'Live',
        icon: GiFlame,
        value: 'live'
    },
    {
        label: 'Ending < 12 hours',
        icon: GiFinishLine,
        value: 'endingSoon'
    },
    {
        label: 'Finished',
        icon: BsStopwatchFill,
        value: 'finished'
    },
]

export default function Filters() {
    const pageSize = useParamsStore(state => state.pageSize);
    const setParams = useParamsStore(state => state.setParams);
    const orderBy = useParamsStore(state => state.orderBy);
    const filterBy = useParamsStore(state => state.filterBy);

    return (
        <div className='flex justify-between items-center mb-4'>

            <div>
                <span className='uppercase text-sm text-gray-500 mr-2'>Filter by</span>
                <Button.Group>
                    {filterButtons.map(({ label, icon: Icon, value }) => (
                        <Button
                            key={value}
                            onClick={() => setParams({ filterBy: value })}
                            color={`${filterBy === value ? 'green' : 'gray'}`}
                            className='focus:ring-1'
                        >
                            <Icon className='mr-3 h-4 w-4' />
                            {label}
                        </Button>
                    ))}
                </Button.Group>
            </div>


            <div>
                <span className='uppercase text-sm text-gray-500 mr-2'>Order by</span>
                <Button.Group>
                    {orderButtons.map(({ label, icon: Icon, value }) => (
                        <Button
                            key={value}
                            onClick={() => setParams({ orderBy: value })}
                            color={`${orderBy === value ? 'green' : 'gray'}`}
                            className='focus:ring-1'
                        >
                            <Icon className='mr-3 h-4 w-4' />
                            {label}
                        </Button>
                    ))}
                </Button.Group>
            </div>

            <div>
                <span className='uppercase text-sm text-gray-500 mr-2'>Page size</span>
                <Button.Group>
                    {pageSizeButtons.map((value, i) => (
                        <Button key={i}
                            onClick={() => setParams({ pageSize: value })}
                            color={`${pageSize === value ? 'green' : 'gray'}`}
                            className='focus:ring-1'
                        >
                            {value}
                        </Button>
                    ))}
                </Button.Group>
            </div>
        </div>
    )
}

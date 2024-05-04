'use client';

import { Auction } from "@/types";
import { Table } from "flowbite-react";

type Props = {
    auction: Auction
}
export default function DetailedSpecs({ auction }: Props) {
    return (
        <Table striped={true} className="shadow-lg font-serif text-lg">
            <Table.Body className="divide-y divide-[#0A6847]">
                <Table.Row className="bg-white dark:border-[#0A6847] dark:bg-gray-800 transition-all duration-500 ease-in-out hover:bg-gray-200">
                    <Table.Cell className="whitespace-nowrap font-medium text-gray-900 dark:text-white">
                        Seller:
                    </Table.Cell>
                    <Table.Cell>
                        {auction.seller}
                    </Table.Cell>
                </Table.Row>
                <Table.Row className="bg-white dark:border-[#0A6847] dark:bg-gray-800 transition-all duration-500 ease-in-out hover:bg-gray-200">
                    <Table.Cell className="whitespace-nowrap font-medium text-gray-900 dark:text-white">
                        Platform:
                    </Table.Cell>
                    <Table.Cell>
                        {auction.platform}
                    </Table.Cell>
                </Table.Row>
                <Table.Row className="bg-white dark:border-[#0A6847] dark:bg-gray-800 transition-all duration-500 ease-in-out hover:bg-gray-200">
                    <Table.Cell className="whitespace-nowrap font-medium text-gray-900 dark:text-white">
                        Title:
                    </Table.Cell>
                    <Table.Cell>
                        {auction.title}
                    </Table.Cell>
                </Table.Row>
                <Table.Row className="bg-white dark:border-[#0A6847] dark:bg-gray-800 transition-all duration-500 ease-in-out hover:bg-gray-200">
                    <Table.Cell className="whitespace-nowrap font-medium text-gray-900 dark:text-white">
                        Year purchased:
                    </Table.Cell>
                    <Table.Cell>
                        {auction.year}
                    </Table.Cell>
                </Table.Row>
                <Table.Row className="bg-white dark:border-[#0A6847] dark:bg-gray-800 transition-all duration-500 ease-in-out hover:bg-gray-200">
                    <Table.Cell className="whitespace-nowrap font-medium text-gray-900 dark:text-white">
                        Hours Played:
                    </Table.Cell>
                    <Table.Cell>
                        {auction.playHours}
                    </Table.Cell>
                </Table.Row>
                <Table.Row className="bg-white dark:border-[#0A6847] dark:bg-gray-800 transition-all duration-500 ease-in-out hover:bg-gray-200">
                    <Table.Cell className="whitespace-nowrap font-medium text-gray-900 dark:text-white">
                        Reserve Price:
                    </Table.Cell>
                    <Table.Cell>
                        {auction.reservePrice > 0 ? 'Yes' : 'No'}
                    </Table.Cell>
                </Table.Row>
            </Table.Body>
        </Table>
    );
}
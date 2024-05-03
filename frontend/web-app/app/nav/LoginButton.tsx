'use client'

import React from 'react'
import { signIn } from 'next-auth/react';
import styled from 'styled-components';

const StyledButton = styled.button`
    background-color: #0A6847;
    color: #F6E9B2;
    padding: 10px 20px;
    font-size: 18px;
    border-radius: 5px;
    box-shadow: 0px 8px 15px rgba(0, 0, 0, 0.1);
    transition: all 0.3s ease 0s;
    cursor: pointer;
    text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.5); // Add text shadow

    &:hover {
        background-color: #7ABA78;
        box-shadow: 0px 15px 20px rgba(46, 229, 157, 0.4);
    }

    &:focus {
        outline: none;
    }
`;

export default function LoginButton() {
    return (
        <StyledButton onClick={() => signIn('id-server', { callbackUrl: '/' })}>
            Login
        </StyledButton>
    )
}
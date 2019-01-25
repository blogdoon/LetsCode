import * as React from 'react';
import { RouteComponentProps } from 'react-router-dom';


export default class Hello extends React.Component<RouteComponentProps<{}>, {}> {
    public render() {
        return <div>
            <h1>Hello dear colleagues</h1>

            <p>Thank you for viewing this page.</p>

        </div>;
    }
}


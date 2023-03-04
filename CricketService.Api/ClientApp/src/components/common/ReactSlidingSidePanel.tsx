import * as React from 'react';
import { useState } from 'react';
import SlidingPanel, { PanelType } from 'react-sliding-side-panel';
import 'react-sliding-side-panel/lib/index.css';

import './ReactSlidingSidePanel.css';

export enum SidePanelType {
    Left = 'left',
    Right = 'right',
    Top = 'top',
    Bottom = 'bottom'
}

interface ReactSlidingSidePanelProps {
    sidePanelType: SidePanelType,
    panelWidth: number,
    isOpen: boolean,
    setIsOpen: (isOpen: boolean) => void,
    children: any;
    isBackDrop?: boolean,
}

export const ReactSlidingSidePanel: React.FunctionComponent<ReactSlidingSidePanelProps> = ({
    sidePanelType,
    panelWidth,
    isOpen,
    setIsOpen,
    isBackDrop = false,
    children
}) => {
    const [panelType, setPanelType] = useState<PanelType>(sidePanelType);
    const [panelSize, setPanelSize] = useState<number>(panelWidth);
    const [noBackdrop, setNoBackdrop] = useState<boolean>(isBackDrop);

    return (
        <div className="sliding-side-panel-container">
            <SlidingPanel
                type={panelType}
                isOpen={isOpen}
                backdropClicked={() => setIsOpen(false)}
                size={panelSize}
                panelClassName="additional-class"
                panelContainerClassName=""
                noBackdrop={noBackdrop}
            >
                <div className="side-panel-container">
                    {
                        children
                    }
                    <button type="button" className="close-button" onClick={() => setIsOpen(false)}>
                        close
                    </button>
                </div>
            </SlidingPanel>
        </div>
    );
};

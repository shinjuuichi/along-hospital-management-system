let grids = [];
const minWidth = 100;

export function init(gridElement, autoFocus) {
    if (gridElement === undefined || gridElement === null) {
        return;
    };

    enableColumnResizing(gridElement);

    let start = gridElement.querySelector('td:first-child');

    if (autoFocus) {
        start.focus();
    }

    const bodyClickHandler = event => {
        const columnOptionsElement = gridElement?.querySelector('.col-options');
        if (columnOptionsElement && event.composedPath().indexOf(columnOptionsElement) < 0) {
            gridElement.dispatchEvent(new CustomEvent('closecolumnoptions', { bubbles: true }));
        }
        const columnResizeElement = gridElement?.querySelector('.col-resize');
        if (columnResizeElement && event.composedPath().indexOf(columnResizeElement) < 0) {
            gridElement.dispatchEvent(new CustomEvent('closecolumnresize', { bubbles: true }));
        }
    };
    const keyboardNavigation = (sibling) => {
        if (sibling !== null) {
            start.focus();
            sibling.focus();
            start = sibling;
        }
    }
    const keyDownHandler = event => {
        const columnOptionsElement = gridElement?.querySelector('.col-options');
        if (columnOptionsElement) {
            if (event.key === "Escape") {
                gridElement.dispatchEvent(new CustomEvent('closecolumnoptions', { bubbles: true }));
                gridElement.focus();
            }
            columnOptionsElement.addEventListener(
                "keydown",
                (event) => {
                    if (event.key === "ArrowRight" || event.key === "ArrowLeft" || event.key === "ArrowDown" || event.key === "ArrowUp") {
                        event.stopPropagation();
                    }
                }
            );
        }
        const columnResizeElement = gridElement?.querySelector('.col-resize');
        if (columnResizeElement) {
            if (event.key === "Escape") {
                gridElement.dispatchEvent(new CustomEvent('closecolumnresize', { bubbles: true }));
                gridElement.focus();
            }
            columnResizeElement.addEventListener(
                "keydown",
                (event) => {
                    if (event.key === "ArrowRight" || event.key === "ArrowLeft" || event.key === "ArrowDown" || event.key === "ArrowUp") {
                        event.stopPropagation();
                    }
                }
            );
        }

        // check if start is a child of gridElement
        if (start !== null && (gridElement.contains(start) || gridElement === start) && document.activeElement === start) {
            const idx = start.cellIndex;

            if (event.key === "ArrowUp") {
                // up arrow
                const previousRow = start.parentElement.previousElementSibling;
                if (previousRow !== null) {
                    event.preventDefault();
                    const previousSibling = previousRow.cells[idx];
                    keyboardNavigation(previousSibling);
                }
            } else if (event.key === "ArrowDown") {
                // down arrow
                const nextRow = start.parentElement.nextElementSibling;
                if (nextRow !== null) {
                    event.preventDefault();
                    const nextSibling = nextRow.cells[idx];
                    keyboardNavigation(nextSibling);
                }
            } else if (event.key === "ArrowLeft") {
                // left arrow
                event.preventDefault();
                const previousSibling = (document.body.dir === '' || document.body.dir === 'ltr') ? start.previousElementSibling : start.nextElementSibling;
                keyboardNavigation(previousSibling);
                event.stopPropagation();
            } else if (event.key === "ArrowRight") {
                // right arrow
                event.preventDefault();
                const nextsibling = (document.body.dir === '' || document.body.dir === 'ltr') ? start.nextElementSibling : start.previousElementSibling;
                keyboardNavigation(nextsibling);
                event.stopPropagation();
            }
        }
        else {
            start = document.activeElement;
        }

    };

    const cells = gridElement.querySelectorAll('[role="gridcell"]');
    cells.forEach((cell) => {
        cell.columnDefinition = {
            columnDataKey: "",
            cellInternalFocusQueue: true,
            cellFocusTargetCallback: (cell) => {
                return cell.children[0];
            }
        }
        cell.addEventListener(
            "keydown",
            (event) => {
                if (event.target.role !== "gridcell" && (event.key === "ArrowRight" || event.key === "ArrowLeft")) {
                    event.stopPropagation();
                }
            }
        );
    });

    document.body.addEventListener('click', bodyClickHandler);
    document.body.addEventListener('mousedown', bodyClickHandler); // Otherwise it seems strange that it doesn't go away until you release the mouse button
    document.body.addEventListener('keydown', keyDownHandler);

    return {
        stop: () => {
            document.body.removeEventListener('click', bodyClickHandler);
            document.body.removeEventListener('mousedown', bodyClickHandler);
            document.body.removeEventListener('keydown', keyDownHandler);
            delete grids[gridElement];
        }
    };
}

export function checkColumnPopupPosition(gridElement, selector) {
    const colPopup = gridElement.querySelector(selector);
    if (colPopup) {
        const gridRect = gridElement.getBoundingClientRect();
        const popupRect = colPopup.getBoundingClientRect();
        const leftOverhang = Math.max(0, gridRect.left - popupRect.left);
        const rightOverhang = Math.max(0, popupRect.right - gridRect.right);
        if (leftOverhang || rightOverhang) {
            const applyOffset = leftOverhang && rightOverhang ? (leftOverhang - rightOverhang) / 2 : (leftOverhang - rightOverhang);
            colPopup.style.transform = `translateX(${applyOffset}px)`;
        }

        colPopup.style.visibility = 'visible';
        colPopup.scrollIntoViewIfNeeded();

        const autoFocusElem = colPopup.querySelector('[autofocus]');
        if (autoFocusElem) {
            autoFocusElem.focus();
        }
    }
}

export function enableColumnResizing(gridElement) {
    const columns = [];
    let min = 75;
    let headerBeingResized;
    let resizeHandle;

    const headers = gridElement.querySelectorAll('.column-header.resizable');

    if (headers.length === 0) {
        return;
    }

    headers.forEach(header => {
        columns.push({
            header,
            size: `minmax(${minWidth}px,auto)`,
        });

        const onPointerMove = (e) => requestAnimationFrame(() => {
            if (!headerBeingResized) {
                return;
            }
            gridElement.style.tableLayout = "fixed";

            const horizontalScrollOffset = document.documentElement.scrollLeft;
            let width;

            if (document.body.dir === '' || document.body.dir === 'ltr') {
                width = (horizontalScrollOffset + e.clientX) - headerBeingResized.getClientRects()[0].x;
            }
            else {
                width = headerBeingResized.getClientRects()[0].x + headerBeingResized.clientWidth - (horizontalScrollOffset + e.clientX);
            }

            const column = columns.find(({ header }) => header === headerBeingResized);
            column.size = Math.max(minWidth, width) + 'px';

            columns.forEach((column) => {
                if (column.size.startsWith('minmax')) {
                    column.size = parseInt(column.header.clientWidth, 10) + 'px';
                }
            });

            gridElement.style.gridTemplateColumns = columns
                .map(({ size }) => size)
                .join(' ');
        });

        const onPointerUp = (e) => {

            window.removeEventListener('pointermove', onPointerMove);
            window.removeEventListener('pointerup', onPointerUp);
            window.removeEventListener('pointercancel', onPointerUp);
            window.removeEventListener('pointerleave', onPointerUp);

            headerBeingResized.classList.remove('header-being-resized');
            headerBeingResized = null;

            if (e.target.hasPointerCapture(e.pointerId)) {
                e.target.releasePointerCapture(e.pointerId);
            }
        };

        const initResize = ({ target, pointerId }) => {
            headerBeingResized = target.parentNode;
            headerBeingResized.classList.add('header-being-resized');


            window.addEventListener('pointermove', onPointerMove);
            window.addEventListener('pointerup', onPointerUp);
            window.addEventListener('pointercancel', onPointerUp);
            window.addEventListener('pointerleave', onPointerUp);

            if (resizeHandle) {
                resizeHandle.setPointerCapture(pointerId);
            }
        };

        header.querySelector('.resize-handle').addEventListener('pointerdown', initResize);

    });

    let initialWidths;
    if (gridElement.style.gridTemplateColumns) {
        initialWidths = gridElement.style.gridTemplateColumns;
    }
    else {
        initialWidths = columns
            .map(({ header, size }) => size)
            .join(' ');

        gridElement.style.gridTemplateColumns = initialWidths;
    }

    let id = gridElement.id;
    grids.push({
        id,
        columns,
        initialWidths
    });
}

export function resetColumnWidths(gridElement) {

    const grid = grids.find(({ id }) => id === gridElement.id);
    if (!grid) {
        return;
    }

    const columnsWidths = grid.initialWidths.split(' ');

    grid.columns.forEach((column, index) => {
        column.size = columnsWidths[index];
    });

    gridElement.style.gridTemplateColumns = grid.initialWidths;
    gridElement.dispatchEvent(new CustomEvent('closecolumnresize', { bubbles: true }));
    gridElement.focus();
}

export function resizeColumnDiscrete(gridElement, column, change) {

    const columns = [];
    let headerBeingResized;

    if (!column) {
        const targetElement = document.activeElement.parentElement.parentElement.parentElement.parentElement;
        if (!(targetElement.classList.contains("column-header") && targetElement.classList.contains("resizable"))) {
            return;
        }
        headerBeingResized = targetElement;
    }
    else {
        headerBeingResized = gridElement.querySelector('.column-header[col-index="' + column + '"]');
    }


    grids.find(({ id }) => id === gridElement.id).columns.forEach(column => {
        if (column.header === headerBeingResized) {
            const width = headerBeingResized.getBoundingClientRect().width + change;

            if (change < 0) {
                column.size = Math.max(minWidth, width) + 'px';
            }
            else {
                column.size = width + 'px';
            }
        }
        else {
            if (column.size.startsWith('minmax')) {
                    column.size = parseInt(column.header.clientWidth, 10) + 'px';
            }
        }
        columns.push(column.size);
    });

    gridElement.style.gridTemplateColumns = columns.join(' ');
}

export function resizeColumnExact(gridElement, column, width) {
    const columns = [];
    let headerBeingResized = gridElement.querySelector('.column-header[col-index="' + column + '"]');

    if (!headerBeingResized) {
        return;
    }

    grids.find(({ id }) => id === gridElement.id).columns.forEach(column => {
        if (column.header === headerBeingResized) {
            column.size = Math.max(minWidth, width) + 'px';
        }
        else {
            if (column.size.startsWith('minmax')) {
                column.size = parseInt(column.header.clientWidth, 10) + 'px';
            }
        }
        columns.push(column.size);
    });

    gridElement.style.gridTemplateColumns = columns.join(' ');

    gridElement.dispatchEvent(new CustomEvent('closecolumnresize', { bubbles: true }));
    gridElement.focus();
}

export function autoFitGridColumns(gridElement, columnCount) {
    let gridTemplateColumns = '';

    for (var i = 0; i < columnCount; i++) {
        const columnWidths = Array
            .from(gridElement.querySelectorAll(`[col-index="${i + 1}"]`))
            .flatMap((x) => x.offsetWidth);

        const maxColumnWidth = Math.max(...columnWidths);

        gridTemplateColumns += ` ${maxColumnWidth}px`;
    }

    gridElement.style.gridTemplateColumns = gridTemplateColumns;
    gridElement.classList.remove("auto-fit");

    grids[gridElement.id] = gridTemplateColumns;
}

function calculateVisibleRows(gridElement, rowHeight) {
    if (rowHeight <= 0) {
        return 0;
    }

    const gridContainer = gridElement.parentElement;

    if (!gridContainer) {
        return 0;
    }

    const availableHeight = gridContainer?.clientHeight || window.visualViewport?.height || window.innerHeight;

    const visibleRows = Math.max(Math.floor(availableHeight / rowHeight), 1);
    return visibleRows;
}

export function dynamicItemsPerPage(gridElement, dotNetObject, rowSize) {
    const observer = new ResizeObserver(() => {
        const visibleRows = calculateVisibleRows(gridElement, rowSize)
        dotNetObject.invokeMethodAsync('UpdateItemsPerPageAsync', visibleRows)
            .catch(err => console.error("Error invoking Blazor method:", err));
    });

    const targetElement = gridElement.parentElement;
    if (targetElement) {
        observer.observe(targetElement);
    }
}

// SIG // Begin signature block
// SIG // MIIoUwYJKoZIhvcNAQcCoIIoRDCCKEACAQExDzANBglg
// SIG // hkgBZQMEAgEFADB3BgorBgEEAYI3AgEEoGkwZzAyBgor
// SIG // BgEEAYI3AgEeMCQCAQEEEBDgyQbOONQRoqMAEEvTUJAC
// SIG // AQACAQACAQACAQACAQAwMTANBglghkgBZQMEAgEFAAQg
// SIG // nb5yb67Ezn0BZhKeb3NtI6GjmefaiQ9cL0RVFiW5qa+g
// SIG // gg2FMIIGAzCCA+ugAwIBAgITMwAABAO91ZVdDzsYrQAA
// SIG // AAAEAzANBgkqhkiG9w0BAQsFADB+MQswCQYDVQQGEwJV
// SIG // UzETMBEGA1UECBMKV2FzaGluZ3RvbjEQMA4GA1UEBxMH
// SIG // UmVkbW9uZDEeMBwGA1UEChMVTWljcm9zb2Z0IENvcnBv
// SIG // cmF0aW9uMSgwJgYDVQQDEx9NaWNyb3NvZnQgQ29kZSBT
// SIG // aWduaW5nIFBDQSAyMDExMB4XDTI0MDkxMjIwMTExM1oX
// SIG // DTI1MDkxMTIwMTExM1owdDELMAkGA1UEBhMCVVMxEzAR
// SIG // BgNVBAgTCldhc2hpbmd0b24xEDAOBgNVBAcTB1JlZG1v
// SIG // bmQxHjAcBgNVBAoTFU1pY3Jvc29mdCBDb3Jwb3JhdGlv
// SIG // bjEeMBwGA1UEAxMVTWljcm9zb2Z0IENvcnBvcmF0aW9u
// SIG // MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA
// SIG // n3RnXcCDp20WFMoNNzt4s9fV12T5roRJlv+bshDfvJoM
// SIG // ZfhyRnixgUfGAbrRlS1St/EcXFXD2MhRkF3CnMYIoeMO
// SIG // MuMyYtxr2sC2B5bDRMUMM/r9I4GP2nowUthCWKFIS1RP
// SIG // lM0YoVfKKMaH7bJii29sW+waBUulAKN2c+Gn5znaiOxR
// SIG // qIu4OL8f9DCHYpME5+Teek3SL95sH5GQhZq7CqTdM0fB
// SIG // w/FmLLx98SpBu7v8XapoTz6jJpyNozhcP/59mi/Fu4tT
// SIG // 2rI2vD50Vx/0GlR9DNZ2py/iyPU7DG/3p1n1zluuRp3u
// SIG // XKjDfVKH7xDbXcMBJid22a3CPbuC2QJLowIDAQABo4IB
// SIG // gjCCAX4wHwYDVR0lBBgwFgYKKwYBBAGCN0wIAQYIKwYB
// SIG // BQUHAwMwHQYDVR0OBBYEFOpuKgJKc+OuNYitoqxfHlrE
// SIG // gXAZMFQGA1UdEQRNMEukSTBHMS0wKwYDVQQLEyRNaWNy
// SIG // b3NvZnQgSXJlbGFuZCBPcGVyYXRpb25zIExpbWl0ZWQx
// SIG // FjAUBgNVBAUTDTIzMDAxMis1MDI5MjYwHwYDVR0jBBgw
// SIG // FoAUSG5k5VAF04KqFzc3IrVtqMp1ApUwVAYDVR0fBE0w
// SIG // SzBJoEegRYZDaHR0cDovL3d3dy5taWNyb3NvZnQuY29t
// SIG // L3BraW9wcy9jcmwvTWljQ29kU2lnUENBMjAxMV8yMDEx
// SIG // LTA3LTA4LmNybDBhBggrBgEFBQcBAQRVMFMwUQYIKwYB
// SIG // BQUHMAKGRWh0dHA6Ly93d3cubWljcm9zb2Z0LmNvbS9w
// SIG // a2lvcHMvY2VydHMvTWljQ29kU2lnUENBMjAxMV8yMDEx
// SIG // LTA3LTA4LmNydDAMBgNVHRMBAf8EAjAAMA0GCSqGSIb3
// SIG // DQEBCwUAA4ICAQBRaP+hOC1+dSKhbqCr1LIvNEMrRiOQ
// SIG // EkPc7D6QWtM+/IbrYiXesNeeCZHCMf3+6xASuDYQ+AyB
// SIG // TX0YlXSOxGnBLOzgEukBxezbfnhUTTk7YB2/TxMUcuBC
// SIG // P45zMM0CVTaJE8btloB6/3wbFrOhvQHCILx41jTd6kUq
// SIG // 4bIBHah3NG0Q1H/FCCwHRGTjAbyiwq5n/pCTxLz5XYCu
// SIG // 4RTvy/ZJnFXuuwZynowyju90muegCToTOwpHgE6yRcTv
// SIG // Ri16LKCr68Ab8p8QINfFvqWoEwJCXn853rlkpp4k7qzw
// SIG // lBNiZ71uw2pbzjQzrRtNbCFQAfmoTtsHFD2tmZvQIg1Q
// SIG // VkzM/V1KCjHL54ItqKm7Ay4WyvqWK0VIEaTbdMtbMWbF
// SIG // zq2hkRfJTNnFr7RJFeVC/k0DNaab+bpwx5FvCUvkJ3z2
// SIG // wfHWVUckZjEOGmP7cecefrF+rHpif/xW4nJUjMUiPsyD
// SIG // btY2Hq3VMLgovj+qe0pkJgpYQzPukPm7RNhbabFNFvq+
// SIG // kXWBX/z/pyuo9qLZfTb697Vi7vll5s/DBjPtfMpyfpWG
// SIG // 0phVnAI+0mM4gH09LCMJUERZMgu9bbCGVIQR7cT5YhlL
// SIG // t+tpSDtC6XtAzq4PJbKZxFjpB5wk+SRJ1gm87olbfEV9
// SIG // SFdO7iL3jWbjgVi1Qs1iYxBmvh4WhLWr48uouzCCB3ow
// SIG // ggVioAMCAQICCmEOkNIAAAAAAAMwDQYJKoZIhvcNAQEL
// SIG // BQAwgYgxCzAJBgNVBAYTAlVTMRMwEQYDVQQIEwpXYXNo
// SIG // aW5ndG9uMRAwDgYDVQQHEwdSZWRtb25kMR4wHAYDVQQK
// SIG // ExVNaWNyb3NvZnQgQ29ycG9yYXRpb24xMjAwBgNVBAMT
// SIG // KU1pY3Jvc29mdCBSb290IENlcnRpZmljYXRlIEF1dGhv
// SIG // cml0eSAyMDExMB4XDTExMDcwODIwNTkwOVoXDTI2MDcw
// SIG // ODIxMDkwOVowfjELMAkGA1UEBhMCVVMxEzARBgNVBAgT
// SIG // Cldhc2hpbmd0b24xEDAOBgNVBAcTB1JlZG1vbmQxHjAc
// SIG // BgNVBAoTFU1pY3Jvc29mdCBDb3Jwb3JhdGlvbjEoMCYG
// SIG // A1UEAxMfTWljcm9zb2Z0IENvZGUgU2lnbmluZyBQQ0Eg
// SIG // MjAxMTCCAiIwDQYJKoZIhvcNAQEBBQADggIPADCCAgoC
// SIG // ggIBAKvw+nIQHC6t2G6qghBNNLrytlghn0IbKmvpWlCq
// SIG // uAY4GgRJun/DDB7dN2vGEtgL8DjCmQawyDnVARQxQtOJ
// SIG // DXlkh36UYCRsr55JnOloXtLfm1OyCizDr9mpK656Ca/X
// SIG // llnKYBoF6WZ26DJSJhIv56sIUM+zRLdd2MQuA3WraPPL
// SIG // bfM6XKEW9Ea64DhkrG5kNXimoGMPLdNAk/jj3gcN1Vx5
// SIG // pUkp5w2+oBN3vpQ97/vjK1oQH01WKKJ6cuASOrdJXtjt
// SIG // 7UORg9l7snuGG9k+sYxd6IlPhBryoS9Z5JA7La4zWMW3
// SIG // Pv4y07MDPbGyr5I4ftKdgCz1TlaRITUlwzluZH9TupwP
// SIG // rRkjhMv0ugOGjfdf8NBSv4yUh7zAIXQlXxgotswnKDgl
// SIG // mDlKNs98sZKuHCOnqWbsYR9q4ShJnV+I4iVd0yFLPlLE
// SIG // tVc/JAPw0XpbL9Uj43BdD1FGd7P4AOG8rAKCX9vAFbO9
// SIG // G9RVS+c5oQ/pI0m8GLhEfEXkwcNyeuBy5yTfv0aZxe/C
// SIG // HFfbg43sTUkwp6uO3+xbn6/83bBm4sGXgXvt1u1L50kp
// SIG // pxMopqd9Z4DmimJ4X7IvhNdXnFy/dygo8e1twyiPLI9A
// SIG // N0/B4YVEicQJTMXUpUMvdJX3bvh4IFgsE11glZo+TzOE
// SIG // 2rCIF96eTvSWsLxGoGyY0uDWiIwLAgMBAAGjggHtMIIB
// SIG // 6TAQBgkrBgEEAYI3FQEEAwIBADAdBgNVHQ4EFgQUSG5k
// SIG // 5VAF04KqFzc3IrVtqMp1ApUwGQYJKwYBBAGCNxQCBAwe
// SIG // CgBTAHUAYgBDAEEwCwYDVR0PBAQDAgGGMA8GA1UdEwEB
// SIG // /wQFMAMBAf8wHwYDVR0jBBgwFoAUci06AjGQQ7kUBU7h
// SIG // 6qfHMdEjiTQwWgYDVR0fBFMwUTBPoE2gS4ZJaHR0cDov
// SIG // L2NybC5taWNyb3NvZnQuY29tL3BraS9jcmwvcHJvZHVj
// SIG // dHMvTWljUm9vQ2VyQXV0MjAxMV8yMDExXzAzXzIyLmNy
// SIG // bDBeBggrBgEFBQcBAQRSMFAwTgYIKwYBBQUHMAKGQmh0
// SIG // dHA6Ly93d3cubWljcm9zb2Z0LmNvbS9wa2kvY2VydHMv
// SIG // TWljUm9vQ2VyQXV0MjAxMV8yMDExXzAzXzIyLmNydDCB
// SIG // nwYDVR0gBIGXMIGUMIGRBgkrBgEEAYI3LgMwgYMwPwYI
// SIG // KwYBBQUHAgEWM2h0dHA6Ly93d3cubWljcm9zb2Z0LmNv
// SIG // bS9wa2lvcHMvZG9jcy9wcmltYXJ5Y3BzLmh0bTBABggr
// SIG // BgEFBQcCAjA0HjIgHQBMAGUAZwBhAGwAXwBwAG8AbABp
// SIG // AGMAeQBfAHMAdABhAHQAZQBtAGUAbgB0AC4gHTANBgkq
// SIG // hkiG9w0BAQsFAAOCAgEAZ/KGpZjgVHkaLtPYdGcimwuW
// SIG // EeFjkplCln3SeQyQwWVfLiw++MNy0W2D/r4/6ArKO79H
// SIG // qaPzadtjvyI1pZddZYSQfYtGUFXYDJJ80hpLHPM8QotS
// SIG // 0LD9a+M+By4pm+Y9G6XUtR13lDni6WTJRD14eiPzE32m
// SIG // kHSDjfTLJgJGKsKKELukqQUMm+1o+mgulaAqPyprWElj
// SIG // HwlpblqYluSD9MCP80Yr3vw70L01724lruWvJ+3Q3fMO
// SIG // r5kol5hNDj0L8giJ1h/DMhji8MUtzluetEk5CsYKwsat
// SIG // ruWy2dsViFFFWDgycScaf7H0J/jeLDogaZiyWYlobm+n
// SIG // t3TDQAUGpgEqKD6CPxNNZgvAs0314Y9/HG8VfUWnduVA
// SIG // KmWjw11SYobDHWM2l4bf2vP48hahmifhzaWX0O5dY0Hj
// SIG // Wwechz4GdwbRBrF1HxS+YWG18NzGGwS+30HHDiju3mUv
// SIG // 7Jf2oVyW2ADWoUa9WfOXpQlLSBCZgB/QACnFsZulP0V3
// SIG // HjXG0qKin3p6IvpIlR+r+0cjgPWe+L9rt0uX4ut1eBrs
// SIG // 6jeZeRhL/9azI2h15q/6/IvrC4DqaTuv/DDtBEyO3991
// SIG // bWORPdGdVk5Pv4BXIqF4ETIheu9BCrE/+6jMpF3BoYib
// SIG // V3FWTkhFwELJm3ZbCoBIa/15n8G9bW1qyVJzEw16UM0x
// SIG // ghomMIIaIgIBATCBlTB+MQswCQYDVQQGEwJVUzETMBEG
// SIG // A1UECBMKV2FzaGluZ3RvbjEQMA4GA1UEBxMHUmVkbW9u
// SIG // ZDEeMBwGA1UEChMVTWljcm9zb2Z0IENvcnBvcmF0aW9u
// SIG // MSgwJgYDVQQDEx9NaWNyb3NvZnQgQ29kZSBTaWduaW5n
// SIG // IFBDQSAyMDExAhMzAAAEA73VlV0POxitAAAAAAQDMA0G
// SIG // CWCGSAFlAwQCAQUAoIGuMBkGCSqGSIb3DQEJAzEMBgor
// SIG // BgEEAYI3AgEEMBwGCisGAQQBgjcCAQsxDjAMBgorBgEE
// SIG // AYI3AgEVMC8GCSqGSIb3DQEJBDEiBCAtTf0anei51v/C
// SIG // IX14OczLKOoo9eCwHJQQE2a9rESzbDBCBgorBgEEAYI3
// SIG // AgEMMTQwMqAUgBIATQBpAGMAcgBvAHMAbwBmAHShGoAY
// SIG // aHR0cDovL3d3dy5taWNyb3NvZnQuY29tMA0GCSqGSIb3
// SIG // DQEBAQUABIIBAC8H6egNTbz/iq3sce3gWhSm5HjVaixe
// SIG // 8aGwXOZ1PWfSAyJLB/tbOVro4Hb7TJw8KJwSHpUEfvkV
// SIG // 8BPqQrZDBveU480ygXLyurUYVP1UuiYxUhfTQv7zbn/5
// SIG // 7hGiCofCbYNrpfQDrlztTA+winza2Jk+CkUuR2b+VArf
// SIG // 4/TQxGXskJI/3V9vUrJuVC+Lc4aUnH1HnxSATgw7VBW6
// SIG // PMqdnwGi9R11431lC4YlweM2FsvzU/SNhGVmuv7qqiQU
// SIG // GPPP41l/zJs9YZmywSb1MXMObWjLvO26orbDGoBNN4TE
// SIG // EiGN7JG+5NC/srisd7XrHkwz2Nb6EVxVUpCXKEoj41/Q
// SIG // C/2hghewMIIXrAYKKwYBBAGCNwMDATGCF5wwgheYBgkq
// SIG // hkiG9w0BBwKggheJMIIXhQIBAzEPMA0GCWCGSAFlAwQC
// SIG // AQUAMIIBWgYLKoZIhvcNAQkQAQSgggFJBIIBRTCCAUEC
// SIG // AQEGCisGAQQBhFkKAwEwMTANBglghkgBZQMEAgEFAAQg
// SIG // YKDhRsbBgr1brbYqdLTtoIs0gsRTKbxRVZXgk5UoD4gC
// SIG // BmguJ1SAKhgTMjAyNTA2MDYxNDI0MzguNjk3WjAEgAIB
// SIG // 9KCB2aSB1jCB0zELMAkGA1UEBhMCVVMxEzARBgNVBAgT
// SIG // Cldhc2hpbmd0b24xEDAOBgNVBAcTB1JlZG1vbmQxHjAc
// SIG // BgNVBAoTFU1pY3Jvc29mdCBDb3Jwb3JhdGlvbjEtMCsG
// SIG // A1UECxMkTWljcm9zb2Z0IElyZWxhbmQgT3BlcmF0aW9u
// SIG // cyBMaW1pdGVkMScwJQYDVQQLEx5uU2hpZWxkIFRTUyBF
// SIG // U046NTIxQS0wNUUwLUQ5NDcxJTAjBgNVBAMTHE1pY3Jv
// SIG // c29mdCBUaW1lLVN0YW1wIFNlcnZpY2WgghH+MIIHKDCC
// SIG // BRCgAwIBAgITMwAAAgAL16p/GyoXVgABAAACADANBgkq
// SIG // hkiG9w0BAQsFADB8MQswCQYDVQQGEwJVUzETMBEGA1UE
// SIG // CBMKV2FzaGluZ3RvbjEQMA4GA1UEBxMHUmVkbW9uZDEe
// SIG // MBwGA1UEChMVTWljcm9zb2Z0IENvcnBvcmF0aW9uMSYw
// SIG // JAYDVQQDEx1NaWNyb3NvZnQgVGltZS1TdGFtcCBQQ0Eg
// SIG // MjAxMDAeFw0yNDA3MjUxODMxMjFaFw0yNTEwMjIxODMx
// SIG // MjFaMIHTMQswCQYDVQQGEwJVUzETMBEGA1UECBMKV2Fz
// SIG // aGluZ3RvbjEQMA4GA1UEBxMHUmVkbW9uZDEeMBwGA1UE
// SIG // ChMVTWljcm9zb2Z0IENvcnBvcmF0aW9uMS0wKwYDVQQL
// SIG // EyRNaWNyb3NvZnQgSXJlbGFuZCBPcGVyYXRpb25zIExp
// SIG // bWl0ZWQxJzAlBgNVBAsTHm5TaGllbGQgVFNTIEVTTjo1
// SIG // MjFBLTA1RTAtRDk0NzElMCMGA1UEAxMcTWljcm9zb2Z0
// SIG // IFRpbWUtU3RhbXAgU2VydmljZTCCAiIwDQYJKoZIhvcN
// SIG // AQEBBQADggIPADCCAgoCggIBAK9V2mnSpD9k5Lp6Exee
// SIG // 9/7ReyiTPQ6Ir93HL9upqp1IZr9gzOfYpBE+Fp0X6OW4
// SIG // hSB3Oi6qyHqgoE/X0/xpLOVSjvdGUFtmr4fzzB55dJGX
// SIG // 1/yOc3VaKFx23VFJD4mXzV7M1rMJi/VJVqPJs8r/S6fU
// SIG // wLcP6FzmEwMXWEqjgeVM89UNwPLgqTZbpkDQyRg2OnEp
// SIG // 9DJWLpF5JQKwoaupfimK5eq/1pzql0pJwAaYIErCd96C
// SIG // 96J5g4jfWFAKWcI5zYfTOpA2p3ks+/P2LQ/9qRqcffy1
// SIG // xC6GsxFBcYcoOCnZqFhjWMHUe/4nfNYHjhEevZeXSb+9
// SIG // Uv5h/i8W+i+vdp/LhJgFcOn1bxPnPMI4GGW5WQjTwMpw
// SIG // pw3bkS3ZNY7MAqo6jXN1/1iMwOxhrOB1EuGCKwFMfB9g
// SIG // PeLwzYgPAFmu2fx0sEwsiIHlW5XV2DNgbcTCqt5J3kaE
// SIG // 9uzUO2O5/GU2gI3uwZX47vN7KRj/0FmDWdcGM2FRkcjq
// SIG // XQPFpsauVfH+a+B2hvcz3MpDsiaUWcvld0RooIRZrAiV
// SIG // wHDM4ju+h4p8AiIyJpwhShifyGy4x+ie3yV6kT24Ph+q
// SIG // 2C2fFwaZlwRR+D02pGVWMQfz/hEGy+SzcNGSDPnrn8Qp
// SIG // Y1eDvpx5DPs4EsfPtOwVWTwSrJaKHm7JoSHATtO+/ZHo
// SIG // XImDAgMBAAGjggFJMIIBRTAdBgNVHQ4EFgQUgCUk2r4J
// SIG // IyqoHucUDl59+X13dzowHwYDVR0jBBgwFoAUn6cVXQBe
// SIG // Yl2D9OXSZacbUzUZ6XIwXwYDVR0fBFgwVjBUoFKgUIZO
// SIG // aHR0cDovL3d3dy5taWNyb3NvZnQuY29tL3BraW9wcy9j
// SIG // cmwvTWljcm9zb2Z0JTIwVGltZS1TdGFtcCUyMFBDQSUy
// SIG // MDIwMTAoMSkuY3JsMGwGCCsGAQUFBwEBBGAwXjBcBggr
// SIG // BgEFBQcwAoZQaHR0cDovL3d3dy5taWNyb3NvZnQuY29t
// SIG // L3BraW9wcy9jZXJ0cy9NaWNyb3NvZnQlMjBUaW1lLVN0
// SIG // YW1wJTIwUENBJTIwMjAxMCgxKS5jcnQwDAYDVR0TAQH/
// SIG // BAIwADAWBgNVHSUBAf8EDDAKBggrBgEFBQcDCDAOBgNV
// SIG // HQ8BAf8EBAMCB4AwDQYJKoZIhvcNAQELBQADggIBACjw
// SIG // hvZ40bSKkPn7hAoMc1jLEDiNx71u7FfT5hFggjlpU7hg
// SIG // iMzYt4m3S2UtG9iAx4NMi67XVbgYtxcVXXrCF7s2MqHy
// SIG // Hv2pUwXVeA4Yoy017QezYDp6Oxtdojt7eo8tYT0qrsxi
// SIG // 68v9phGQcCLEqEtg/h/txwicTw8oczBaj/qZZbTwAgf0
// SIG // DcGe6vhxsmb97/Hrfq0GIPLBdz07lng4N3Uf85NTWsCf
// SIG // 3XxQg2JVjXggQi7zT0AXHjGFxURSoXElMLO5hXSAw4Wa
// SIG // casiCg9lg8BcjSBhHs5/p3eJF0bqXjRMfnkqSV8pUQ/t
// SIG // XeOYW+j8ziBewZHD7UbRVtsF4JIy6rU1lpQZL85drjX2
// SIG // Cdwj2VWg8jA2ml4Dvh+g4q7CeCBvYpCHfeNfplg3o5I+
// SIG // WmJ/UDekTn6PxzR4NbYpsKRaFIr6gBbuoq1mRcOVfsi6
// SIG // /BS3O52zGtpRUosc7ves3Zw7DyJs9HOkrW2MoSkpTN7g
// SIG // 0YvVFsnUiqpxG7SejJPmLsb86a5LlkCWFn6T77oPsE54
// SIG // qMpFcHNMkVXLHeMTM5550bWQxjElBJfbTFZ3m2EbIcGS
// SIG // MiU7AYC2ZhzO6tkxSv1/feOEpCKsmNtgHLi3tBqqDXwE
// SIG // giHGbc22f8z+JU9vzdKQ259n3wM42ZISPkK6q/fN5kGV
// SIG // sGXa905NTGBJQ04c9g9DMIIHcTCCBVmgAwIBAgITMwAA
// SIG // ABXF52ueAptJmQAAAAAAFTANBgkqhkiG9w0BAQsFADCB
// SIG // iDELMAkGA1UEBhMCVVMxEzARBgNVBAgTCldhc2hpbmd0
// SIG // b24xEDAOBgNVBAcTB1JlZG1vbmQxHjAcBgNVBAoTFU1p
// SIG // Y3Jvc29mdCBDb3Jwb3JhdGlvbjEyMDAGA1UEAxMpTWlj
// SIG // cm9zb2Z0IFJvb3QgQ2VydGlmaWNhdGUgQXV0aG9yaXR5
// SIG // IDIwMTAwHhcNMjEwOTMwMTgyMjI1WhcNMzAwOTMwMTgz
// SIG // MjI1WjB8MQswCQYDVQQGEwJVUzETMBEGA1UECBMKV2Fz
// SIG // aGluZ3RvbjEQMA4GA1UEBxMHUmVkbW9uZDEeMBwGA1UE
// SIG // ChMVTWljcm9zb2Z0IENvcnBvcmF0aW9uMSYwJAYDVQQD
// SIG // Ex1NaWNyb3NvZnQgVGltZS1TdGFtcCBQQ0EgMjAxMDCC
// SIG // AiIwDQYJKoZIhvcNAQEBBQADggIPADCCAgoCggIBAOTh
// SIG // pkzntHIhC3miy9ckeb0O1YLT/e6cBwfSqWxOdcjKNVf2
// SIG // AX9sSuDivbk+F2Az/1xPx2b3lVNxWuJ+Slr+uDZnhUYj
// SIG // DLWNE893MsAQGOhgfWpSg0S3po5GawcU88V29YZQ3MFE
// SIG // yHFcUTE3oAo4bo3t1w/YJlN8OWECesSq/XJprx2rrPY2
// SIG // vjUmZNqYO7oaezOtgFt+jBAcnVL+tuhiJdxqD89d9P6O
// SIG // U8/W7IVWTe/dvI2k45GPsjksUZzpcGkNyjYtcI4xyDUo
// SIG // veO0hyTD4MmPfrVUj9z6BVWYbWg7mka97aSueik3rMvr
// SIG // g0XnRm7KMtXAhjBcTyziYrLNueKNiOSWrAFKu75xqRdb
// SIG // Z2De+JKRHh09/SDPc31BmkZ1zcRfNN0Sidb9pSB9fvzZ
// SIG // nkXftnIv231fgLrbqn427DZM9ituqBJR6L8FA6PRc6ZN
// SIG // N3SUHDSCD/AQ8rdHGO2n6Jl8P0zbr17C89XYcz1DTsEz
// SIG // OUyOArxCaC4Q6oRRRuLRvWoYWmEBc8pnol7XKHYC4jMY
// SIG // ctenIPDC+hIK12NvDMk2ZItboKaDIV1fMHSRlJTYuVD5
// SIG // C4lh8zYGNRiER9vcG9H9stQcxWv2XFJRXRLbJbqvUAV6
// SIG // bMURHXLvjflSxIUXk8A8FdsaN8cIFRg/eKtFtvUeh17a
// SIG // j54WcmnGrnu3tz5q4i6tAgMBAAGjggHdMIIB2TASBgkr
// SIG // BgEEAYI3FQEEBQIDAQABMCMGCSsGAQQBgjcVAgQWBBQq
// SIG // p1L+ZMSavoKRPEY1Kc8Q/y8E7jAdBgNVHQ4EFgQUn6cV
// SIG // XQBeYl2D9OXSZacbUzUZ6XIwXAYDVR0gBFUwUzBRBgwr
// SIG // BgEEAYI3TIN9AQEwQTA/BggrBgEFBQcCARYzaHR0cDov
// SIG // L3d3dy5taWNyb3NvZnQuY29tL3BraW9wcy9Eb2NzL1Jl
// SIG // cG9zaXRvcnkuaHRtMBMGA1UdJQQMMAoGCCsGAQUFBwMI
// SIG // MBkGCSsGAQQBgjcUAgQMHgoAUwB1AGIAQwBBMAsGA1Ud
// SIG // DwQEAwIBhjAPBgNVHRMBAf8EBTADAQH/MB8GA1UdIwQY
// SIG // MBaAFNX2VsuP6KJcYmjRPZSQW9fOmhjEMFYGA1UdHwRP
// SIG // ME0wS6BJoEeGRWh0dHA6Ly9jcmwubWljcm9zb2Z0LmNv
// SIG // bS9wa2kvY3JsL3Byb2R1Y3RzL01pY1Jvb0NlckF1dF8y
// SIG // MDEwLTA2LTIzLmNybDBaBggrBgEFBQcBAQROMEwwSgYI
// SIG // KwYBBQUHMAKGPmh0dHA6Ly93d3cubWljcm9zb2Z0LmNv
// SIG // bS9wa2kvY2VydHMvTWljUm9vQ2VyQXV0XzIwMTAtMDYt
// SIG // MjMuY3J0MA0GCSqGSIb3DQEBCwUAA4ICAQCdVX38Kq3h
// SIG // LB9nATEkW+Geckv8qW/qXBS2Pk5HZHixBpOXPTEztTnX
// SIG // wnE2P9pkbHzQdTltuw8x5MKP+2zRoZQYIu7pZmc6U03d
// SIG // mLq2HnjYNi6cqYJWAAOwBb6J6Gngugnue99qb74py27Y
// SIG // P0h1AdkY3m2CDPVtI1TkeFN1JFe53Z/zjj3G82jfZfak
// SIG // Vqr3lbYoVSfQJL1AoL8ZthISEV09J+BAljis9/kpicO8
// SIG // F7BUhUKz/AyeixmJ5/ALaoHCgRlCGVJ1ijbCHcNhcy4s
// SIG // a3tuPywJeBTpkbKpW99Jo3QMvOyRgNI95ko+ZjtPu4b6
// SIG // MhrZlvSP9pEB9s7GdP32THJvEKt1MMU0sHrYUP4KWN1A
// SIG // PMdUbZ1jdEgssU5HLcEUBHG/ZPkkvnNtyo4JvbMBV0lU
// SIG // ZNlz138eW0QBjloZkWsNn6Qo3GcZKCS6OEuabvshVGtq
// SIG // RRFHqfG3rsjoiV5PndLQTHa1V1QJsWkBRH58oWFsc/4K
// SIG // u+xBZj1p/cvBQUl+fpO+y/g75LcVv7TOPqUxUYS8vwLB
// SIG // gqJ7Fx0ViY1w/ue10CgaiQuPNtq6TPmb/wrpNPgkNWcr
// SIG // 4A245oyZ1uEi6vAnQj0llOZ0dFtq0Z4+7X6gMTN9vMvp
// SIG // e784cETRkPHIqzqKOghif9lwY1NNje6CbaUFEMFxBmoQ
// SIG // tB1VM1izoXBm8qGCA1kwggJBAgEBMIIBAaGB2aSB1jCB
// SIG // 0zELMAkGA1UEBhMCVVMxEzARBgNVBAgTCldhc2hpbmd0
// SIG // b24xEDAOBgNVBAcTB1JlZG1vbmQxHjAcBgNVBAoTFU1p
// SIG // Y3Jvc29mdCBDb3Jwb3JhdGlvbjEtMCsGA1UECxMkTWlj
// SIG // cm9zb2Z0IElyZWxhbmQgT3BlcmF0aW9ucyBMaW1pdGVk
// SIG // MScwJQYDVQQLEx5uU2hpZWxkIFRTUyBFU046NTIxQS0w
// SIG // NUUwLUQ5NDcxJTAjBgNVBAMTHE1pY3Jvc29mdCBUaW1l
// SIG // LVN0YW1wIFNlcnZpY2WiIwoBATAHBgUrDgMCGgMVAIyT
// SIG // ny2W94r4qS97Ei5VhWy61o5koIGDMIGApH4wfDELMAkG
// SIG // A1UEBhMCVVMxEzARBgNVBAgTCldhc2hpbmd0b24xEDAO
// SIG // BgNVBAcTB1JlZG1vbmQxHjAcBgNVBAoTFU1pY3Jvc29m
// SIG // dCBDb3Jwb3JhdGlvbjEmMCQGA1UEAxMdTWljcm9zb2Z0
// SIG // IFRpbWUtU3RhbXAgUENBIDIwMTAwDQYJKoZIhvcNAQEL
// SIG // BQACBQDr7RODMCIYDzIwMjUwNjA2MDcxMzA3WhgPMjAy
// SIG // NTA2MDcwNzEzMDdaMHcwPQYKKwYBBAGEWQoEATEvMC0w
// SIG // CgIFAOvtE4MCAQAwCgIBAAICCUMCAf8wBwIBAAICEoEw
// SIG // CgIFAOvuZQMCAQAwNgYKKwYBBAGEWQoEAjEoMCYwDAYK
// SIG // KwYBBAGEWQoDAqAKMAgCAQACAwehIKEKMAgCAQACAwGG
// SIG // oDANBgkqhkiG9w0BAQsFAAOCAQEACwnl3zfUOE6c53so
// SIG // lzvgrPAO1ulhigjgxekwoaEXXFzLNPftZKJo4AOrUFk7
// SIG // BP/sIKzLy7R+Rhp+0Qjs1l53JdXKtcmerKDKb+GEcC7W
// SIG // oKJSQkL+4eaZ2ZAce/1y7uY62ZXcc3K+leQLY75StjpB
// SIG // Ohk5corByWAW42chDi1jl/FmvT4EiamuOJOvu3GukDfl
// SIG // eUreFjzjZSHpBMCB5TSLI6DEtiYTHKtF/WDJptb9yXd1
// SIG // YUjYp18j1pfowKSQcTzmbk6h5yT/H6qb4lNh00S5J2J7
// SIG // Aujmm1LvygE6FtHkEzQgqTrJ2s/ZbCLmUeCdB/ljp61V
// SIG // ytZqcTynDIDSQOPA5zGCBA0wggQJAgEBMIGTMHwxCzAJ
// SIG // BgNVBAYTAlVTMRMwEQYDVQQIEwpXYXNoaW5ndG9uMRAw
// SIG // DgYDVQQHEwdSZWRtb25kMR4wHAYDVQQKExVNaWNyb3Nv
// SIG // ZnQgQ29ycG9yYXRpb24xJjAkBgNVBAMTHU1pY3Jvc29m
// SIG // dCBUaW1lLVN0YW1wIFBDQSAyMDEwAhMzAAACAAvXqn8b
// SIG // KhdWAAEAAAIAMA0GCWCGSAFlAwQCAQUAoIIBSjAaBgkq
// SIG // hkiG9w0BCQMxDQYLKoZIhvcNAQkQAQQwLwYJKoZIhvcN
// SIG // AQkEMSIEINVAd3QFAhw8r/av9BObVtLuLsBTp7LwpPBY
// SIG // 6zIQ6XQMMIH6BgsqhkiG9w0BCRACLzGB6jCB5zCB5DCB
// SIG // vQQg1Mjt7DWd27qwTQxlAleDXzNoB+GlrkbnSJP/SgJP
// SIG // 2ScwgZgwgYCkfjB8MQswCQYDVQQGEwJVUzETMBEGA1UE
// SIG // CBMKV2FzaGluZ3RvbjEQMA4GA1UEBxMHUmVkbW9uZDEe
// SIG // MBwGA1UEChMVTWljcm9zb2Z0IENvcnBvcmF0aW9uMSYw
// SIG // JAYDVQQDEx1NaWNyb3NvZnQgVGltZS1TdGFtcCBQQ0Eg
// SIG // MjAxMAITMwAAAgAL16p/GyoXVgABAAACADAiBCCJa7Qj
// SIG // nDGBp0BsvsLCSBS6PjrV+sAlrWSx9mdNBsyS9DANBgkq
// SIG // hkiG9w0BAQsFAASCAgAkfM7+8aQMxBSoBE+3d0k4KLtV
// SIG // tufMpbdK4Wqm7eVDVNJbwisHjxjpm3pC9M65YMbZpzSn
// SIG // jvr50zgDyCz+PlU91XIAb/3QT/uQAIoVILNunbcDBw6O
// SIG // zgkGZ0uSOYD7RYZP08ql70w0B2FxLoo8fVvBLAdI99v/
// SIG // 0P7MojV4DlyvNzZ1dHRL4mb31VWopXzgCd9UMVM50Hut
// SIG // Cbq/bkgWDWlPPMcfak9FuZrwmX7gY7o+AIMz0lQs9cKV
// SIG // qiCFbEcBNxda6qQmxMgkIlpe80qZe78gQ3iEMEt9C1UT
// SIG // ki/blTqT/Fn+J4i02wWAiBhLwBHoJKQJS2g/1iqpABsc
// SIG // GcH6J9F4uNhAet4Fw8Q0AUCeJC10xxcwiNnVGAFrQhUl
// SIG // eeqpSO69NAEUfJ6qYRdV1NsGHeGaQTCMStxZf/UpXHb6
// SIG // By1H8skyOo2UJObuoJF5g5LsL2aYAa0z24hmsvrSAG24
// SIG // 0pkfzjYrcm9F0BGHpmOvJsnDtjNAUJfC/E5843zgv3Sg
// SIG // i8j1zKGLMEbFCKahQzMtoJfZTCwS3qz21+iCi3Chbxc6
// SIG // MQNMIpmBok/07/hvAbXFPpR6LqZGvPJKziQ0MbUw7GvG
// SIG // cYZA4q7CmjDmkUMue39LE5n85OEkTPf7vD6DAQ+PPSNL
// SIG // w21iClv8zzLtPHcFxAygRLfgNKx77eU3ZqjR708G/w==
// SIG // End signature block

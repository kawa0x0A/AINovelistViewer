export async function copyText(text) {
    try {
        await navigator.clipboard.writeText(text);
    } catch (error) {
        console.error(error.message);
    }
}

export async function pasteText(text) {
    try {
        return await navigator.clipboard.readText();
    } catch (error) {
        console.error(error.message);
    }
}
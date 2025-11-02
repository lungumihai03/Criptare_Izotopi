Criptare Izotopi – A Custom Substitution-Permutation Cipher
===========================================================

Overview
--------

**Criptare Izotopi** is a Windows Forms application developed in C# that implements a **custom symmetric encryption scheme** combining **character-to-code substitution** with **matrix-based row and column permutations**. The system is designed for educational purposes and demonstrates fundamental cryptographic concepts such as substitution, block ciphers, and permutation matrices.

The name _"Izotopi"_ (Romanian for "isotopes") reflects the idea that each character in the plaintext is represented by multiple unique numeric "isotopes" (codes), from which one is randomly selected during encryption.

* * *

Features
--------

* **Dynamic Alphabet Generation**: Each unique character in the message is assigned n unique 3-digit codes (e.g., 123, 456, 789), where n is the security level.
* **Security Level Control**: Adjustable via a NumericUpDown (1–10), determining how many alternative codes each character has.
* **Substitution Cipher**: Randomly selects one code per character during encryption.
* **Permutation Matrix (ALFA)**: A user-defined permutation applied to both **rows and columns** of a square matrix.
* **Block Processing**: Message codes are arranged into an n×n matrix (padded with 0s if needed).
* **Detailed Logging**: Full step-by-step encryption/decryption process saved to criptare_detaliat.txt and decriptare_detaliat.txt.
* **Dictionary Persistence**: Substitution table saved to dictionar.txt and formatted version in dictionar_formatat.txt.
* **Decryption Support**: Reverses permutations using the **inverse of ALFA** and maps codes back to characters.
* **Reset Functionality**: Clears all data and UI fields.

* * *

How It Works
------------

### Encryption Process

1. **Generate Dictionary**: For each unique character in the message, generate nivel unique 3-digit numbers (99–998).
2. **Encode Message**: Replace each character with a **randomly chosen** code from its list.
3. **Form Matrix**: Arrange codes into an n×n matrix (where n = ceil(sqrt(message_length))).
4. **Apply ALFA Permutation**:
   * Permute **rows** according to user-provided ALFA.
   * Permute **columns** using the same ALFA.
5. **Output**: Final matrix displayed and saved with full trace.

### Decryption Process

1. **Load Dictionary** from dictionar.txt.
2. **Parse Encrypted Matrix** from output.
3. **Compute Inverse ALFA**: Reverse the permutation order.
4. **Reverse Permutations**:
   * Undo **column** permutation using ALFA⁻¹.
   * Undo **row** permutation using ALFA⁻¹.
5. **Extract Codes**: Read non-zero values from the restored matrix.
6. **Map Back to Characters**: Match each code to its original character using the dictionary.

* * *

Usage
-----

### 1. **Encryption**

1. Enter your message in **"Mesaj"** (top RichTextBox).
2. Set **"nivel de securitate"** (1–10).
3. Enter a permutation in **"alfa"** (bottom-left TextBox) — exactly n unique numbers from 1 to n, space-separated (e.g., 3 1 4 2).
4. Click **"Cripteaza"**.
5. Encrypted matrix appears in **"Mesajul criptat"**.
6. Files generated:
   * dictionar.txt – character → code mapping
   * dictionar_formatat.txt – tabular view (opens in Notepad)
   * criptare_detaliat.txt – full encryption steps

### 2. **Decryption**

1. Ensure dictionar.txt is present in the application folder.
2. Paste the encrypted matrix into **"Mesajul criptat"**.
3. Enter the **same ALFA permutation** used in encryption in the bottom **"alfa"** field.
4. Click **"Decripteaza"**.
5. Decrypted message is shown in a message box and saved in decriptare_detaliat.txt.

### 3. **Reset**

Click **"Reset"** to clear all fields and dictionaries.

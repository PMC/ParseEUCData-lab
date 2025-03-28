# ParseEUCData-lab

## Overview

ParseEUCData-lab is a C# project designed to Parse a csv file (EUC_1.csv) and then download images from websites using 10 asynchronous Tasks. The primary modules of this project are `FilteredV1.cs` and `Program.cs`. This project focuses on efficient data extraction, filtering, and processing tailored for EUC-related datasets into json format.

## Key Features
- **Data Source Aggregation**: Supports multiple EUC specific data sources.
- **Filtering Mechanism**: Provides a robust method to filter relevant data based on specific criteria.
- **Scalability**: Designed to handle large volumes of data with minimal performance degradation.
- **Maintainability**: Well-documented codebase for ease of future development and collaboration.

## Components

### FilteredV1.cs
This module is responsible for filtering EUC data from a CSV file. It includes classes and methods tailored to process and extract useful information from the raw EUC datasets. Key functionalities include:
- Data validation
- Conditional filtering based on predefined rules
- Output generation in a structured json format and extraction of Images from websites included in the dataset.

#### Usage Instructions
Refer to [FilteredV1.cs](https://raw.githubusercontent.com/PMC/ParseEUCData-lab/refs/heads/master/FilteredV1.cs) for detailed implementation and usage instructions.

### Program.cs
The entry point of the project, `Program.cs` orchestrates the overall data processing workflow. It integrates various components to perform a comprehensive parsing and filtering task. Key functionalities include:
- Data ingestion from CSV file and Images from websites
- Invocation of the filtering mechanism defined in `FilteredV1.cs`
- Generation of final output as a pretty formated jaon file for further data manipulation
#### Usage Instructions
Refer to [Program.cs](https://raw.githubusercontent.com/PMC/ParseEUCData-lab/refs/heads/master/Program.cs) for detailed implementation and usage instructions.

## Getting Started

1. **Clone the Repository**
   ```bash
   git clone https://github.com/PMC/ParseEUCData-lab.git
   ```

2. **Navigate to the Project Directory**
   ```bash
   cd ParseEUCData-lab
   ```

3. **Build and Run the Project**
   - Open the project in your preferred IDE (e.g., Visual Studio).
   - Build the solution.
   - Execute `Program.cs` to start the data processing workflow.

## Output
The directory <ins> bin\Debug\net9.0\data\ </ins> contains the "result.json" file and Directories where the images have
been downloaded. 

Images are in [Brand]-[Model] structure

## License

ParseEUCData-lab is licensed under the MIT License. See the [LICENSE](LICENSE) file for more details.

## Glossary

EUC stands for Electric Unicycles and are the best Electric mobility device there is in the hole freaking world!! Nothing comes close!!
